using System.Text.RegularExpressions;
using Apollo.Data;
using Apollo.ViewModels;
using System.Linq;
using Apollo.Entities;
using System;
using System.IO;
using Microsoft.Extensions.Hosting.Internal;
using System.Collections.Generic;

namespace Apollo.Services 
{
    public class PlayerService
    {
        readonly ApolloContext _db;
        readonly AuthenticationService _authenticationService; 

        public PlayerService(
            ApolloContext db,
            AuthenticationService authenticationService
        )
        {
            _db = db;
            _authenticationService = authenticationService;
        }

        public bool PlayerRegisterFormDataControl(PlayerRegisterViewModel playerVM)
        {
            playerVM.Name = playerVM.Name.Trim();
            playerVM.Surname = playerVM.Surname.Trim();
            playerVM.Nickname = playerVM.Nickname.Trim();
            playerVM.PhoneNumber = playerVM.PhoneNumber.Trim();
            playerVM.MailAddress = playerVM.MailAddress.Trim();
            playerVM.Password = playerVM.Password.Trim();
            playerVM.PasswordVerify = playerVM.PasswordVerify.Trim();
            Regex regForMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"); 
            Regex regForPhone = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
            if(
                (!String.IsNullOrEmpty(playerVM.Name)) &&
                (!String.IsNullOrEmpty(playerVM.Surname)) &&
                (!String.IsNullOrEmpty(playerVM.Nickname)) &&
                (!String.IsNullOrEmpty(playerVM.PhoneNumber)) &&
                (!String.IsNullOrEmpty(playerVM.MailAddress)) &&
                (!String.IsNullOrEmpty(playerVM.Password)) &&
                (!String.IsNullOrEmpty(playerVM.PasswordVerify)) &&
                (playerVM.Password == playerVM.PasswordVerify) && 
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

        public bool PlayerAuthenticator(string JWT)
        {
            return _authenticationService.VerfiyToken(JWT);
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
                    string profilePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", playerVM.ProfilePhoto.FileName);
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
                    playerData.ProfilePhotoId = newProfilePhoto.Id;
                    playerData.TwitterContact = playerVM.TwitterContact.Trim();
                    playerData.FacebookContact = playerVM.FacebookContact.Trim();
                    playerData.YoutubeContact = playerVM.YoutubeContact.Trim();
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
                        string photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", image.FileName);
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
    }
}