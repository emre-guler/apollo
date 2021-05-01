using System.Text.RegularExpressions;
using Apollo.Data;
using Apollo.ViewModels;
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
                (playerVM.BirthDate != null) &&
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

        public Player PlayerLoginControl(PlayerLoginViewModel playerVM)
        {
            Player player =  _db.Players
                .Where(x => x.MailAddress == playerVM.MailAddress)
                .FirstOrDefault();
            bool passwordControl = BCrypt.Net.BCrypt.Verify(playerVM.Password, player.Password);
            if(player != null && passwordControl)
            {
                return player;
            }
            else 
            {
                return null;
            }
        }

        public PlayerViewModel PlayerLogin(Player player)
        {
            PlayerViewModel pVM = new PlayerViewModel();
            return pVM;
        }
    }
}