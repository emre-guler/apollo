using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Apollo.Data;
using Apollo.Entities;
using Apollo.Enums;
using Apollo.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Apollo.Services
{
    public class TeamService
    {
        private string webSiteUrl = "http://localhost:5001/";
        private readonly ApolloContext _db;
        private readonly AuthenticationService _authenticationService ;
        private readonly GeneralMethodsService _methodService;
        readonly MailService _mailService;
        public TeamService(
            ApolloContext db,
            AuthenticationService authenticationService,
            MailService mailService,
            GeneralMethodsService methodService
        )
        {
            _db = db;
            _authenticationService = authenticationService;
            _mailService = mailService;
            _methodService = methodService;
        }

        public bool TeamRegisterFormDataControl(TeamRegisterViewModel teamVM)
        {
            try
            {
                teamVM.TeamName = teamVM.TeamName.Trim();
                teamVM.PhoneNumber = teamVM.PhoneNumber.Trim();
                teamVM.Password = teamVM.Password.Trim();
                teamVM.MailAddress = teamVM.MailAddress.Trim();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Data);
                return false;
            }
            Regex regForMail = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"); 
            Regex regForPhone = new(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
            if(
                (!String.IsNullOrEmpty(teamVM.TeamName)) &&
                (!String.IsNullOrEmpty(teamVM.PhoneNumber)) &&
                (!String.IsNullOrEmpty(teamVM.Password)) &&
                (!String.IsNullOrEmpty(teamVM.MailAddress)) &&
                (teamVM.Password.Length >= 8) &&
                (regForMail.Match(teamVM.MailAddress).Success) && 
                (regForPhone.Match(teamVM.PhoneNumber).Success)
            )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> NewAccountControl(string mailAddress, string phoneNumber)
        {
            bool teamControl = await _db.Teams
                .AnyAsync(x => x.MailAddress == mailAddress || x.PhoneNumber == phoneNumber);
            if(teamControl)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task CreateTeam(TeamRegisterViewModel teamVM)
        {
            string profilePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/team", teamVM.ProfilePhoto.FileName);
            using(Stream stream = new FileStream(profilePhotoPath, FileMode.Create))
            {
                await teamVM.ProfilePhoto.CopyToAsync(stream);
            };
            _db.Teams.Add(new Team {
                TeamName = teamVM.TeamName,
                CreatedAt = DateTime.Now,
                MailAddress = teamVM.MailAddress,
                Password = BCrypt.Net.BCrypt.HashPassword(teamVM.Password),
                PhoneNumber = teamVM.TeamName,
                ProfilePhotoPath = profilePhotoPath
            });
            await _db.SaveChangesAsync();
            await _mailService.TeamWelcomeMail(teamVM.MailAddress);
        }

        public async Task<Team> TeamLoginControl(LoginViewModel teamVM)
        {
            if(!String.IsNullOrEmpty(teamVM.MailAddress) || !String.IsNullOrEmpty(teamVM.Password))
            {
                Team team = await _db.Teams
                    .Where(x => x.MailAddress == teamVM.MailAddress)
                    .FirstOrDefaultAsync();
                bool passwordControl = BCrypt.Net.BCrypt.Verify(teamVM.Password, team.Password);
                if(team is not null && passwordControl)
                {
                    return team;
                }
            }
            return null;
        }

        public string TeamLogin(int teamId)
        {
            return _authenticationService.GenerateToken(teamId);
        }

        public bool TeamAuthenticator(string JWT, int teamId)
        {
            string newToken = _authenticationService.GenerateToken(teamId);
            if(newToken == JWT)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> TeamMailVerificationControl(int teamId)
        {
            return await _db.Teams.AnyAsync(x => !x.DeletedAt.HasValue && !x.IsMailVerified && x.Id == teamId);
        }

        public async Task SendMailVerification(int teamId)
        {
            Team teamData = _db.Teams
                .FirstOrDefault(x => !x.DeletedAt.HasValue && x.Id == teamId);
            int confirmationCode = _methodService.GenerateRandomInt();
            string url = _methodService.GenerateRandomString();
            webSiteUrl = webSiteUrl + url;
            await _mailService.TeamSendMailVerification(confirmationCode, webSiteUrl, teamData.MailAddress);
            _db.VerificationRequests.Add(new VerificationRequest {
                UserType = UserType.Team,
                UserId = teamData.Id,
                URL = url,
                CreatedAt = DateTime.Now,
                ConfirmationCode = confirmationCode
            });
            await _db.SaveChangesAsync();
        }

        public async Task<bool> TeamMailConfirmation(int confirmationCode, string hashedData)
        {
            var verification = await _db.VerificationRequests
                .LastOrDefaultAsync(
                    x => !x.DeletedAt.HasValue &&
                    x.CreatedAt.Value.AddHours(1) > DateTime.Now &&
                    x.ConfirmationCode == confirmationCode &&
                    x.URL == hashedData &&
                    x.UserType == UserType.Team
                );
            if(verification is not null)
            {
                verification.DeletedAt = DateTime.Now;
                Team teamData = await _db.Teams.FirstOrDefaultAsync(x => !x.DeletedAt.HasValue && x.Id == verification.UserId);
                teamData.IsMailVerified = true;
                await _db.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> TeamMailVerificationPageControl(string hashedData)
        {
            return await _db.VerificationRequests
                .AnyAsync(
                    x => !x.DeletedAt.HasValue &&
                    x.CreatedAt.Value.AddHours(1) > DateTime.Now &&
                    x.URL == hashedData &&
                    x.UserType == UserType.Team
                );
        }

        public async Task<bool> FreezeTeamAccount(int teamId)
        {
            Team teamData = await _db.Teams
                .FirstOrDefaultAsync(x => !x.DeletedAt.HasValue && x.Id == teamId);
            if(teamData is not null) 
            {
                teamData.DeletedAt =  DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            else 
            {
                return false;
            }
        }
    }
}