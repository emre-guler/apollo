using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Apollo.Data;
using Apollo.Entities;
using Apollo.ViewModels;

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
            Player player = _db.Players.FirstOrDefault(x => !x.DeletedAt.HasValue && x.MailAddress == playerMailAddress);
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
            mailServer.Send(message);
        }
        public async Task TeamWelcomeMail(string teamMailAddress)
        {
            string teamName = _db.Teams.FirstOrDefault(x => !x.DeletedAt.HasValue && x.MailAddress == teamMailAddress).TeamName ?? "";
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
            mailServer.Send(message);
        }

        public async Task TeamSendMailVerification(int confirmationCode, string url, string teamMailAdress)
        {
            string teamName = _db.Teams.FirstOrDefault(x => !x.DeletedAt.HasValue && x.MailAddress == teamMailAdress).TeamName ?? "";
            UserMailVerificationViewModel viewModel = new()
            {
                Name = teamName,
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

            message.To.Add(teamMailAdress);
            mailServer.Send(message);
        }

        public async Task PlayerSendMailVerification(int confirmationCode, string url, string playerMailAdress)
        {
            string playerName =_db.Players.FirstOrDefault(x => !x.DeletedAt.HasValue && x.MailAddress  == playerMailAdress).Name ?? "";
            UserMailVerificationViewModel viewModel = new()
            {
                Name = playerName,
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

            message.To.Add(playerMailAdress);
            mailServer.Send(message);
        }

    }
}