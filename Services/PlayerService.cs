using System.Text.RegularExpressions;
using Apollo.Data;
using Apollo.ViewModels;
using System.Linq;
using Apollo.Services;
using Apollo.Entities;
using System;
using System.IO;
using Microsoft.Extensions.Hosting.Internal;
using System.Collections.Generic;
using Apollo.Enums;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Apollo.DTO;

namespace Apollo.Services
{
    public class PlayerService
    {
        private string webSiteUrl = "http://localhost:5001/";
        private readonly ApolloContext _db;
        private readonly AuthenticationService _authenticationService;
        private readonly MailService _mailService;
        private readonly GeneralMethodsService _methodService;

        public PlayerService(
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

        public bool PlayerRegisterFormDataControl(PlayerRegisterViewModel playerVM)
        {
            try
            {
                playerVM.Name = playerVM.Name.Trim();
                playerVM.Surname = playerVM.Surname.Trim();
                playerVM.Nickname = playerVM.Nickname.Trim();
                playerVM.PhoneNumber = playerVM.PhoneNumber.Trim();
                playerVM.MailAddress = playerVM.MailAddress.Trim();
                playerVM.Password = playerVM.Password.Trim();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data);
                return false;
            }
            Regex regForMail = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex regForPhone = new(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
            if (
                (!String.IsNullOrEmpty(playerVM.Name)) &&
                (!String.IsNullOrEmpty(playerVM.Surname)) &&
                (!String.IsNullOrEmpty(playerVM.Nickname)) &&
                (!String.IsNullOrEmpty(playerVM.PhoneNumber)) &&
                (!String.IsNullOrEmpty(playerVM.MailAddress)) &&
                (!String.IsNullOrEmpty(playerVM.Password)) &&
                (playerVM.Password.Length >= 8) &&
                (regForMail.Match(playerVM.MailAddress).Success) &&
                (regForPhone.Match(playerVM.PhoneNumber).Success) &&
                (playerVM.CityId != 0) &&
                (playerVM.Gender != null)
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
            bool userControl = await _db.Players
                .Where(x => x.MailAddress == mailAddress || x.PhoneNumber == phoneNumber)
                .AnyAsync();
            if (!userControl)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task CreatePlayer(PlayerRegisterViewModel playerVM)
        {
            _db.Players.Add(new Player
            {
                BirtDate = playerVM.BirthDate,
                CityId = playerVM.CityId,
                CreatedAt = DateTime.Now,
                MailAddress = playerVM.MailAddress,
                Name = playerVM.Name,
                Password = BCrypt.Net.BCrypt.HashPassword(playerVM.Password),
                PhoneNumber = playerVM.PhoneNumber,
                Nickname = playerVM.Nickname,
                Surname = playerVM.Surname,
                Gender = playerVM.Gender ?? Enums.Gender.Man
            });
            await _db.SaveChangesAsync();
            await _mailService.PlayerWelcomeMail(playerVM.MailAddress);
        }

        public async Task<Player> PlayerLoginControl(LoginViewModel playerVM)
        {
            if (!String.IsNullOrEmpty(playerVM.MailAddress) || !String.IsNullOrEmpty(playerVM.Password))
            {
                Player player = await _db.Players
                    .Where(x => x.MailAddress == playerVM.MailAddress && !x.DeletedAt.HasValue)
                    .FirstOrDefaultAsync();
                if (player is not null)
                {
                    bool passwordControl = BCrypt.Net.BCrypt.Verify(playerVM.Password, player.Password);
                    if (passwordControl)
                    {
                        return player;
                    }
                }
            }
            return null;
        }

        public string PlayerLogin(Player playerData)
        {
            JwtClaimDTO data = new()
            {
                Id = playerData.Id,
                Name = playerData.Name,
                MailAddress = playerData.MailAddress,
                UserType = UserType.Player
            };
            return _authenticationService.GenerateToken(data);
        }

        public async Task<bool> BuilUpYourProfile(PlayerBuildUpViewModel playerVM, int playerId)
        {
            Player playerData = await _db.Players
                .Where(x => !x.DeletedAt.HasValue && x.Id == playerId)
                .FirstOrDefaultAsync();
            if (playerData is not null)
            {
                string profilePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/player", playerVM.ProfilePhoto.FileName);
                using (Stream stream = new FileStream(profilePhotoPath, FileMode.Create))
                {
                    await playerVM.ProfilePhoto.CopyToAsync(stream);
                };
                Photo newProfilePhoto = new()
                {
                    PhotoPath = profilePhotoPath,
                    CreatedAt = DateTime.Now
                };
                _db.Photos.Add(newProfilePhoto);
                await _db.SaveChangesAsync();
                playerData.TwitterContact = playerVM.TwitterContact.Trim();
                playerData.FacebookContact = playerVM.FacebookContact.Trim();
                playerData.YoutubeContact = playerVM.YoutubeContact.Trim();
                playerData.SteamContact = playerVM.SteamContact.Trim();
                playerData.LolContact = playerVM.LolContact.Trim();
                playerData.ValorantContact = playerVM.ValorantContact.Trim();
                playerData.TwitchContact = playerVM.TwitchContact.Trim();
                playerData.ProfilePhotoId = newProfilePhoto.Id;
                playerData.SalaryException = playerVM.SalaryException;
                playerData.IsActiveForTeam = playerVM.IsActiveForTeam;
                foreach (var cityId in playerVM.AvailableCities)
                {
                    _db.PlayerCities.Add(new PlayerCity
                    {
                        CityId = cityId,
                        CreatedAt = DateTime.Now,
                        PlayerId = playerId
                    });
                }
                foreach (var game in playerVM.PlayerGames)
                {
                    _db.PlayerGames.Add(new PlayerGame
                    {
                        GameType = game,
                        PlayerId = playerId,
                        CreatedAt = DateTime.Now
                    });
                }
                await _db.SaveChangesAsync();
                foreach (var image in playerVM.UserPhotos)
                {
                    string photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/player", image.FileName);
                    using (Stream stream = new FileStream(photoPath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    };
                    Photo newPhoto = new Photo
                    {
                        PhotoPath = photoPath,
                        CreatedAt = DateTime.Now
                    };
                    await _db.SaveChangesAsync();
                    _db.PlayerPhotos.Add(new PlayerPhoto
                    {
                        PhotoId = newPhoto.Id,
                        PlayerId = playerId,
                        CreatedAt = DateTime.Now
                    });
                }
                await _db.SaveChangesAsync();
                foreach (var video in playerVM.UserVideos)
                {
                    string videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/video", video.FileName);
                    using (Stream stream = new FileStream(videoPath, FileMode.Create))
                    {
                        video.CopyTo(stream);
                    }
                    Video newVideo = new()
                    {
                        VideoPath = videoPath,
                        CreatedAt = DateTime.Now
                    };
                    _db.Videos.Add(newVideo);
                    await _db.SaveChangesAsync();
                    _db.PlayerVideos.Add(new PlayerVideo
                    {
                        VideoId = newVideo.Id,
                        CreatedAt = DateTime.Now,
                        PlayerId = playerId
                    });
                }
                await _db.SaveChangesAsync();
                foreach (var achievement in playerVM.TournamentAchievements)
                {
                    _db.Achievements.Add(new Achievement
                    {
                        TournamentId = achievement.TournamentId,
                        Rank = achievement.Rank,
                        PlayerId = playerId,
                        CreatedAt = DateTime.Now
                    });
                }
                await _db.SaveChangesAsync();
                foreach (var oldTeam in playerVM.OldTeams)
                {
                    _db.OldTeams.Add(new Entities.OldTeam
                    {
                        TeamName = oldTeam.TeamName,
                        Description = oldTeam.Description,
                        StartedAt = oldTeam.StartedAt,
                        EndedAt = oldTeam.EndedAt,
                        Game = oldTeam.Game,
                        PlayerId = playerId,
                        CreatedAt = DateTime.Now
                    });
                }
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> PlayerMailVerificationControl(int userId)
        {
            return await _db.Players.AnyAsync(x => !x.DeletedAt.HasValue && !x.IsMailVerified && x.Id == userId);
        }

        public async Task SendMailVerification(int userId)
        {
            Player playerData = await _db.Players
                .FirstOrDefaultAsync(x => !x.DeletedAt.HasValue && x.Id == userId);

            int confirmationCode = _methodService.GenerateRandomInt();
            string url = await _methodService.GenerateRandomString(RequestType.MailVerification);
            webSiteUrl = webSiteUrl + url;
            await _mailService.PlayerSendMailVerification(confirmationCode, webSiteUrl, playerData);
            _db.VerificationRequests.Add(new VerificationRequest
            {
                UserType = UserType.Player,
                UserId = playerData.Id,
                URL = url,
                CreatedAt = DateTime.Now,
                ConfirmationCode = confirmationCode
            });
            await _db.SaveChangesAsync();
        }

        public async Task<bool> PlayerMailConfirmation(int confirmationCode, string hashedData)
        {
            var verification = await _db.VerificationRequests
                .LastOrDefaultAsync(
                    x => !x.DeletedAt.HasValue &&
                    x.CreatedAt.Value.AddHours(1) > DateTime.Now &&
                    x.ConfirmationCode == confirmationCode &&
                    x.URL == hashedData &&
                    x.UserType == UserType.Player
                );
            if (verification is not null)
            {
                verification.DeletedAt = DateTime.Now;
                Player playerData = await _db.Players.FirstOrDefaultAsync(x => !x.DeletedAt.HasValue && x.Id == verification.UserId);
                playerData.IsMailVerified = true;
                await _db.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> PlayerMailVerificationPageControl(string hashedData)
        {
            return await _db.VerificationRequests
                .AnyAsync(
                    x => !x.DeletedAt.HasValue &&
                    x.CreatedAt.Value.AddHours(1) > DateTime.Now &&
                    x.URL == hashedData &&
                    x.UserType == UserType.Player
                );
        }

        public async Task<bool> PlayerUpdateState(int userId)
        {
            Player playerData = await _db.Players
                .FirstOrDefaultAsync(x => !x.DeletedAt.HasValue && x.Id == userId);
            if (playerData is not null)
            {
                playerData.IsActiveForTeam = !playerData.IsActiveForTeam;
                await _db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> FreezePlayerAccount(int userId)
        {
            Player playerData = await _db.Players
                .FirstOrDefaultAsync(x => !x.DeletedAt.HasValue && x.Id == userId);
            if (playerData is not null)
            {
                playerData.DeletedAt = DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> PlayerControlByMail(string mailAddress)
        {
            return await _db.Players
                .Where(x => !x.DeletedAt.HasValue && x.MailAddress == mailAddress)
                .AnyAsync();
        }

        public async Task SendPasswordCode(string mailAddress)
        {
            Player playerData = await _db.Players
                .Where(x => !x.DeletedAt.HasValue && x.MailAddress == mailAddress)
                .FirstOrDefaultAsync();

            int confirmationCode = _methodService.GenerateRandomInt();
            string url = await _methodService.GenerateRandomString(RequestType.ResetPassword);
            webSiteUrl = webSiteUrl + url;

            await _mailService.PlayerSendPasswordCode(confirmationCode, webSiteUrl, playerData);
            _db.PasswordResetRequests.Add(new PasswordResetRequest
            {
                ConfirmationCode = confirmationCode,
                CreatedAt = DateTime.Now,
                URL = webSiteUrl,
                UserId = playerData.Id,
                UserType = UserType.Team
            });
            await _db.SaveChangesAsync();
        }

        public async Task<bool> PlayerForgetPasswordConfirmationPageControl(string hashedData)
        {
            return await _db.PasswordResetRequests
                .AnyAsync(
                    x => !x.DeletedAt.HasValue &&
                    x.CreatedAt.Value.AddHours(1) > DateTime.Now &&
                    x.URL == hashedData &&
                    x.UserType == UserType.Player
                );
        }

        public async Task<bool> PlayerResetPassword(int confirmationCode, string hashedData, string password)
        {
            PasswordResetRequest resetPassword = await _db.PasswordResetRequests
                .LastOrDefaultAsync(
                    x => !x.DeletedAt.HasValue &&
                    x.CreatedAt.Value.AddHours(1) > DateTime.Now &&
                    x.ConfirmationCode == confirmationCode &&
                    x.URL == hashedData &&
                    x.UserType == UserType.Player
                );
            if (resetPassword is not null)
            {
                resetPassword.DeletedAt = DateTime.Now;
                Player playerData = await _db.Players.FirstOrDefaultAsync(x => !x.DeletedAt.HasValue && x.Id == resetPassword.UserId);
                playerData.Password = BCrypt.Net.BCrypt.HashPassword(password);

                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}