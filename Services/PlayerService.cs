using System.Text.RegularExpressions;
using Apollo.Data;
using Apollo.ViewModelds;
using System.Linq;
using Apollo.Entities;
using System;

namespace Apollo.Services 
{
    public class PlayerService
    {
        readonly ApolloContext _db;

        public PlayerService(
            ApolloContext db
        )
        {
            _db = db;
        }

        public bool PlayerRegisterFormDataControl(PlayerRegisterViewModel playerVM)
        {
            if(
                (playerVM.Name != null || playerVM.Name != "") &&
                (playerVM.Surname.Trim() != null || playerVM.Surname.Trim() != "") &&
                (playerVM.Nickname.Trim() != null || playerVM.Nickname.Trim() != "") &&
                (playerVM.PhoneNumber.Trim() != null || playerVM.PhoneNumber.Trim() != "") &&
                (playerVM.MailAddress.Trim() != null || playerVM.MailAddress.Trim() != "") &&
                (playerVM.Password.Trim() != null || playerVM.Password.Trim() != "") &&
                (playerVM.PasswordVerify.Trim() != null || playerVM.PasswordVerify.Trim() != "") &&
                (playerVM.CityId != 0) &&
                (playerVM.BirthDate != null)
            )
            {
                if(playerVM.Password.Trim() == playerVM.PasswordVerify.Trim() && playerVM.Password.Trim().Length >= 8)
                {
                    Regex regForMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"); 
                    Regex regForPhone = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
                    Match mailControl = regForMail.Match(playerVM.MailAddress);
                    Match phoneControl = regForPhone.Match(playerVM.PhoneNumber);
                    if(mailControl.Success && phoneControl.Success)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else 
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool NewAccountControl(string mailAddress, string phoneNumber)
        {
            var users = _db.Players
                .FirstOrDefault(x => x.MailAddress == mailAddress.Trim() || x.PhoneNumber == phoneNumber.Trim());
            if(users == null)
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
                Surname = playerVM.Surname
            });
            _db.SaveChanges();
        }
    }
}