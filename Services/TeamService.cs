using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Apollo.Data;
using Apollo.Entities;
using Apollo.Enums;
using Apollo.ViewModels;

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

        public bool NewAccountControl(string mailAddress, string phoneNumber)
        {
            bool teamControl = _db.Teams
                .Any(x => x.MailAddress == mailAddress || x.PhoneNumber == phoneNumber);
            if(teamControl)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateTeam(TeamRegisterViewModel teamVM)
        {
            string profilePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/team", teamVM.ProfilePhoto.FileName);
            using(Stream stream = new FileStream(profilePhotoPath, FileMode.Create))
            {
                teamVM.ProfilePhoto.CopyTo(stream);
            };
            _db.Teams.Add(new Team {
                TeamName = teamVM.TeamName,
                CreatedAt = DateTime.Now,
                MailAddress = teamVM.MailAddress,
                Password = BCrypt.Net.BCrypt.HashPassword(teamVM.Password),
                PhoneNumber = teamVM.TeamName,
                ProfilePhotoPath = profilePhotoPath
            });
            _db.SaveChanges();
            _mailService.TeamWelcomeMail(teamVM.MailAddress);
        }

        public Team TeamLoginControl(LoginViewModel teamVM)
        {
            if(!String.IsNullOrEmpty(teamVM.MailAddress) || !String.IsNullOrEmpty(teamVM.Password))
            {
                Team team = _db.Teams
                    .Where(x => x.MailAddress == teamVM.MailAddress)
                    .FirstOrDefault();
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

        public bool TeamMailVerificationControl(int teamId)
        {
            return _db.Teams.Any(x => !x.DeletedAt.HasValue && !x.IsMailVerified && x.Id == teamId);
        }

        public void SendMailVerification(int teamId)
        {
            Team teamData = _db.Teams
                .FirstOrDefault(x => !x.DeletedAt.HasValue && x.Id == teamId);
            int confirmationCode = _methodService.GenerateRandomInt();
            string url = _methodService.GenerateRandomString();
            webSiteUrl = webSiteUrl + url;
            _mailService.UserSendMailVerification(confirmationCode, webSiteUrl, teamData.MailAddress);
            _db.VerificationRequests.Add(new VerificationRequest {
                UserType = UserType.Team,
                UserId = teamData.Id,
                URL = url,
                CreatedAt = DateTime.Now,
                ConfirmationCode = confirmationCode
            });
            _db.SaveChanges();
        }

        public bool TeamMailConfirmation(int confirmationCode, string hashedData)
        {
            var verification = _db.VerificationRequests
                .LastOrDefault(
                    x => !x.DeletedAt.HasValue &&
                    x.CreatedAt.Value.AddHours(1) > DateTime.Now &&
                    x.ConfirmationCode == confirmationCode &&
                    x.URL == hashedData &&
                    x.UserType == UserType.Team
                );
            if(verification is not null)
            {
                verification.DeletedAt = DateTime.Now;
                Team teamData = _db.Teams.FirstOrDefault(x => !x.DeletedAt.HasValue && x.Id == verification.UserId);
                teamData.IsMailVerified = true;
                _db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TeamMailVerificationPageControl(string hashedData)
        {
            return _db.VerificationRequests
                .Any(
                    x => !x.DeletedAt.HasValue &&
                    x.CreatedAt.Value.AddHours(1) > DateTime.Now &&
                    x.URL == hashedData &&
                    x.UserType == UserType.Team
                );
        }
    }
}