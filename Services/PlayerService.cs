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
            try {
                playerVM.Name = playerVM.Name.Trim();
                playerVM.Surname = playerVM.Surname.Trim();
                playerVM.Nickname = playerVM.Nickname.Trim();
                playerVM.PhoneNumber = playerVM.PhoneNumber.Trim();
                playerVM.MailAddress = playerVM.MailAddress.Trim();
                playerVM.Password = playerVM.Password.Trim();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Data);
                return false;
            }
            Regex regForMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"); 
            Regex regForPhone = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
            if(
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

        public bool NewAccountControl(string mailAddress, string phoneNumber)
        {
            bool userControl = _db.Players
                .Any(x => x.MailAddress == mailAddress || x.PhoneNumber == phoneNumber);
            if(userControl)
            {
                return true;
            }   
            else 
            {
                return false;
            }
        }

        public void CreatePlayer(PlayerRegisterViewModel playerVM)
        {
            _db.Players.Add(new Player {
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
            _db.SaveChanges();
            _mailService.PlayerWelcomeMail(playerVM.MailAddress);
        }
        
        public Player PlayerLoginControl(LoginViewModel playerVM)
        {
            if(!String.IsNullOrEmpty(playerVM.MailAddress) || !String.IsNullOrEmpty(playerVM.Password))
            {
                Player player =  _db.Players
                    .Where(x => x.MailAddress == playerVM.MailAddress && !x.DeletedAt.HasValue)
                    .FirstOrDefault();
                if(player != null)
                {
                    bool passwordControl = BCrypt.Net.BCrypt.Verify(playerVM.Password, player.Password);
                    if(passwordControl)
                    {
                        return player;
                    }
                }
            }
            return null;
        }

        public string PlayerLogin(int playerId)
        {
           return  _authenticationService.GenerateToken(playerId);
        }

        public bool PlayerAuthenticator(string JWT, int userId)
        {
            string newToken = _authenticationService.GenerateToken(userId);
            if(newToken == JWT)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool BuilUpYourProfile(PlayerBuildUpViewModel playerVM, string userJWT, string playerId)
        {
            int id = Int16.Parse(playerId);
            string newToken = _authenticationService.GenerateToken(id);
            if(userJWT == newToken)
            {
                Player playerData = _db.Players
                    .FirstOrDefault(x => !x.DeletedAt.HasValue && x.Id == id);
                
                if(playerData != null)
                {
                    string profilePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/player", playerVM.ProfilePhoto.FileName);
                    using (Stream stream = new FileStream(profilePhotoPath, FileMode.Create))
                    {
                        playerVM.ProfilePhoto.CopyTo(stream);
                    };
                    Photo newProfilePhoto = new Photo()
                    {
                        PhotoPath = profilePhotoPath,
                        CreatedAt = DateTime.Now
                    };
                    _db.Photos.Add(newProfilePhoto);
                    _db.SaveChanges();
                    playerData.TwitterContact = playerVM.TwitterContact.Trim();
                    playerData.FacebookContact = playerVM.FacebookContact.Trim();
                    playerData.YoutubeContact = playerVM.YoutubeContact.Trim();
                    playerData.ProfilePhotoId = newProfilePhoto.Id;
                    playerData.SalaryException = playerVM.SalaryException;
                    playerData.IsActiveForTeam = playerVM.IsActiveForTeam;
                    foreach(var cityId in playerVM.AvailableCities)
                    {
                        _db.PlayerCities.Add(new PlayerCity 
                        {
                            CityId = cityId,
                            CreatedAt = DateTime.Now,
                            PlayerId = id
                        });
                    }
                    foreach(var game in playerVM.PlayerGames)
                    {
                        _db.PlayerGames.Add(new PlayerGame 
                        {
                            GameType = game,
                            PlayerId = id,
                            CreatedAt = DateTime.Now
                        });
                    }
                    _db.SaveChanges();
                    foreach(var image in playerVM.UserPhotos)
                    {
                        string photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/player", image.FileName);
                        using(Stream stream = new FileStream(photoPath, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        };
                        Photo newPhoto = new Photo
                        {
                            PhotoPath = photoPath,
                            CreatedAt = DateTime.Now
                        };
                        _db.SaveChanges();
                        _db.PlayerPhotos.Add(new PlayerPhoto 
                        {
                            PhotoId = newPhoto.Id,
                            PlayerId = id,
                            CreatedAt = DateTime.Now
                        });
                    }
                    _db.SaveChanges();
                    foreach(var video in playerVM.UserVideos)
                    {
                        string videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/video", video.FileName);
                        using(Stream stream = new FileStream(videoPath, FileMode.Create))
                        {
                            video.CopyTo(stream);
                        }
                        Video newVideo = new Video
                        {
                            VideoPath = videoPath,
                            CreatedAt = DateTime.Now
                        };
                        _db.SaveChanges();
                        _db.PlayerVideos.Add(new PlayerVideo 
                        {
                            VideoId = newVideo.Id,
                            CreatedAt = DateTime.Now,
                            PlayerId = id
                        });
                    }
                    _db.SaveChanges();
                    foreach(var achievement in playerVM.TournamentAchievements)
                    {
                        _db.Achievements.Add(new Achievement 
                        {
                            TournamentId = achievement.TournamentId,
                            Rank = achievement.Rank,
                            PlayerId = id,
                            CreatedAt = DateTime.Now
                        });
                    }
                    _db.SaveChanges();
                    foreach(var oldTeam in playerVM.OldTeams)
                    {
                        _db.OldTeams.Add(new Entities.OldTeam 
                        {
                            TeamName = oldTeam.TeamName,
                            Description = oldTeam.Description,
                            StartedAt = oldTeam.StartedAt,
                            EndedAt = oldTeam.EndedAt,
                            Game = oldTeam.Game,
                            PlayerId = id,
                            CreatedAt = DateTime.Now
                        });
                    }
                    _db.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool PlayerMailVerificationControl(int userId)
        {
            return _db.Players.Any(x => !x.DeletedAt.HasValue && !x.IsMailVerified && x.Id == userId);
        }

        public void SendMailVerification(int userId)
        {
            Player playerData = _db.Players
                .FirstOrDefault(x => !x.DeletedAt.HasValue && x.Id == userId);

            int confirmationCode = _methodService.GenerateRandomInt();
            string url = _methodService.GenerateRandomString();            
            webSiteUrl = webSiteUrl + url;
            _mailService.UserSendMailVerification(confirmationCode, webSiteUrl, playerData.MailAddress);
            _db.VerificationRequests.Add(new VerificationRequest {
                UserType = UserType.Player,
                UserId = playerData.Id,
                URL = url,
                CreatedAt = DateTime.Now,
                ConfirmationCode = confirmationCode
            });
            _db.SaveChanges();
        }

        public bool PlayerMailConfirmation(int confirmationCode, string hashedData)
        {
            var verification = _db.VerificationRequests
                .LastOrDefault(
                    x => !x.DeletedAt.HasValue && 
                    x.CreatedAt.Value.AddHours(1) > DateTime.Now &&
                    x.ConfirmationCode == confirmationCode &&
                    x.URL == hashedData &&
                    x.UserType == UserType.Player
                );
            if(verification != null)
            {
                verification.DeletedAt = DateTime.Now;
                Player playerData = _db.Players.FirstOrDefault(x => !x.DeletedAt.HasValue && x.Id == verification.UserId);
                playerData.IsMailVerified = true;
                _db.SaveChanges();
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PlayerMailVerificationPageControl(string hashedData)
        {
            return _db.VerificationRequests
                .Any(
                    x => !x.DeletedAt.HasValue &&
                    x.CreatedAt.Value.AddHours(1) > DateTime.Now &&
                    x.URL == hashedData &&
                    x.UserType == UserType.Player
                );
        }
    }
}