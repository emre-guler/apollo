using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Apollo.Data;
using Apollo.Entities;
using Apollo.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Apollo.Services
{
    public class MailService
    {
        private string webSiteUrl = "http://localhost:5001/";
        private string senderMailAddress = "softwareionicc@hotmail.com";
        private string senderMailPass = "kekobabam123";
        private int mailServerPort = 587;
        private string mailServerHost = "smtp.live.com";
        private bool mailServerSSL = true;
        private readonly ViewRenderService _viewRenderService;
        private readonly ApolloContext _db;

        public MailService(
            ViewRenderService viewRenderService,
            ApolloContext db
        )
        {
            _viewRenderService = viewRenderService;
            _db = db;
        }

        public async Task PlayerWelcomeMail(string playerMailAddress)
        {
            Player player = await _db.Players.FirstOrDefaultAsync(x => !x.DeletedAt.HasValue && x.MailAddress == playerMailAddress);
            UserMailVerificationViewModel viewModel = new() 
            {
                Name = player.Name,
                Link = webSiteUrl + $"player/{player.Nickname}"
            };
            var bodyResult = await _viewRenderService.RenderToStringAsync("~/Pages/Mailing/PlayerWelcomeMail.cshtml", viewModel);
            MailMessage message = new()
            {
                From = new MailAddress(senderMailAddress),
                Subject = "Apollo | Yeni Bir Maceraya Hoş Geldiniz",
                Body = bodyResult
            };
            SmtpClient mailServer = new()
            {
                Credentials = new NetworkCredential(senderMailAddress , senderMailPass),
                Port = mailServerPort,
                Host = mailServerHost,
                EnableSsl = mailServerSSL
            };

            message.To.Add(playerMailAddress);
            await mailServer.SendMailAsync(message);
        }
        public async Task TeamWelcomeMail(string teamMailAddress)
        {
            Team team = await _db.Teams.FirstOrDefaultAsync(x => !x.DeletedAt.HasValue && x.MailAddress == teamMailAddress);
            string teamName = team.TeamName ?? "";
            UserMailVerificationViewModel viewModel = new()
            {
                Name = teamName,
                Link = webSiteUrl + $"team/{teamName}"
            };
            var bodyResult = await _viewRenderService.RenderToStringAsync("~/Pages/Mailing/TeamWelcomeMail.cshtml", viewModel);
            MailMessage message = new()
            {
                From = new MailAddress(senderMailAddress),
                Subject = "Apollo | Yeni Bir Maceraya Hoş Geldiniz",
                Body = bodyResult
            };
            SmtpClient mailServer = new()
            {
                Credentials = new NetworkCredential(senderMailAddress , senderMailPass),
                Port = mailServerPort,
                Host = mailServerHost,
                EnableSsl = mailServerSSL
            };
            
            message.To.Add(teamMailAddress);
            await mailServer.SendMailAsync(message);
        }

        public async Task TeamSendMailVerification(int confirmationCode, string url, Team teamData)
        {
            UserMailVerificationViewModel viewModel = new()
            {
                Name = teamData.TeamName,
                ConfirmationCode = confirmationCode,
                Link = url
            };
            var bodyResult = await _viewRenderService.RenderToStringAsync("~/Pages/Mailing/TeamMailActiviation.cshtml", viewModel);
            MailMessage message = new()
            {
                From = new MailAddress(senderMailAddress),
                Subject = "Apollo | Mail Doğrulaması",
                Body = bodyResult
            };
            SmtpClient mailServer = new()
            {
                Credentials = new NetworkCredential(senderMailAddress, senderMailPass),
                Port = mailServerPort,
                Host = mailServerHost,
                EnableSsl = mailServerSSL
            };

            message.To.Add(teamData.MailAddress);
            await mailServer.SendMailAsync(message);
        }

        public async Task PlayerSendMailVerification(int confirmationCode, string url, Player playerData)
        {
            UserMailVerificationViewModel viewModel = new()
            {
                Name = playerData.Name,
                ConfirmationCode = confirmationCode,
                Link = url
            };
            var bodyResult = await _viewRenderService.RenderToStringAsync("~/Pages/Mailing/PlayerMailActiviation.cshtml", viewModel);
            MailMessage message = new()
            {
                From = new MailAddress(senderMailAddress),
                Subject = "Apollo | Mail Doğrulaması",
                Body = bodyResult
            };
            SmtpClient mailServer = new()
            {
                Credentials = new NetworkCredential(senderMailAddress, senderMailPass),
                Port = mailServerPort,
                Host = mailServerHost,
                EnableSsl = mailServerSSL
            };

            message.To.Add(playerData.MailAddress);
            await mailServer.SendMailAsync(message);
        }

        public async Task PlayerSendPasswordCode(int confirmationCode, string url, Player playerData)
        {
            PlayerResetPasswordViewModel viewModel = new()
            {
                Name = playerData.Name ?? "",
                ConfirmationCode = confirmationCode,
                Link = url
            };
            var bodyResult = await _viewRenderService.RenderToStringAsync("~/Pages/Mailing/PlayerForgetPasswordMail.cshtml", viewModel);
            MailMessage message = new()
            {
                From = new MailAddress(senderMailAddress),
                Subject = "Apollo | Şifre Sıfırlama",
                Body = bodyResult
            };
            SmtpClient mailServer = new()
            {
                Credentials = new NetworkCredential(senderMailAddress, senderMailPass),
                Port = mailServerPort,
                Host = mailServerHost,
                EnableSsl = mailServerSSL
            };

            message.To.Add(playerData.MailAddress);
            await mailServer.SendMailAsync(message);
        }
    }
}