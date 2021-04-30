using System;
using System.Text.RegularExpressions;
using Apollo.Data;
using Apollo.Entities;
using Apollo.ViewModelds;

namespace Apollo.Services
{
    public class TeamService
    {
        readonly ApolloContext _db;

        public TeamService(
            ApolloContext db
        )
        {
            _db = db;
        }

        public bool TeamRegisterFormDataControl(TeamRegisterViewModel teamVM)
        {
            if(
                (teamVM.TeamName.Trim() != null || teamVM.TeamName.Trim() != "") &&
                (teamVM.PhoneNumber.Trim() != null || teamVM.PhoneNumber.Trim() != "") &&
                (teamVM.Password.Trim() != null || teamVM.Password.Trim() != "") &&
                (teamVM.PasswordVerification.Trim() != null || teamVM.PasswordVerification.Trim() != "") &&
                (teamVM.MailAddress.Trim() != null || teamVM.MailAddress.Trim() != "")
            )
            {
                if(teamVM.Password.Trim() == teamVM.PasswordVerification.Trim() && teamVM.Password.Trim().Length >= 8)
                {
                    Regex regForMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"); 
                    Regex regForPhone = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
                    Match mailControl = regForMail.Match(teamVM.MailAddress);
                    Match phoneControl = regForPhone.Match(teamVM.PhoneNumber);
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

        public void CreateTeam(TeamRegisterViewModel teamVM)
        {
            _db.Teams.Add(new Team {
                CreatedAt = DateTime.Now,
                MailAddress = teamVM.MailAddress,
                Password = BCrypt.Net.BCrypt.HashPassword(teamVM.Password),
                PhoneNumber = teamVM.TeamName,
                
            });
            _db.SaveChanges();
        }
    }
}