using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Apollo.Entities;

namespace Apollo.Services
{
    public class MailService
    {
        private string senderMailAddress = "softwareionicc@hotmail.com";
        private string senderMailPass = "kekobabam123";
        private int mailServerPort = 587;
        private string mailServerHost = "smtp.live.com";
        private bool mailServerSSL = true;
        private readonly ViewRenderService _viewRenderService;

        public MailService(
            ViewRenderService viewRenderService
        )
        {
            _viewRenderService = viewRenderService;
        }

        public async Task PlayerWelcomeMail(string playerMailAddress)
        {
            var bodyResult = await _viewRenderService.RenderToStringAsync("~/Views/Mailing/PlayerWelcomeMail.cshtml", new Player());
            MailMessage message = new()
            {
                From = new MailAddress(senderMailAddress),
                Subject = "Welcome / Hoşgeldiniz",
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
            var bodyResult = await _viewRenderService.RenderToStringAsync("~/Views/Mailing/TeamWelcomeMail.cshtml", new Team());
            MailMessage message = new()
            {
                From = new MailAddress(senderMailAddress),
                Subject = "Welcome / Hoşgeldiniz",
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

        public async Task UserSendMailVerification(int confirmationCode, string url, string userMailAdress)
        {
            var bodyResult = await _viewRenderService.RenderToStringAsync("~/Views/Mailing/UserWelcomeMail.cshtml", new object());
            MailMessage message = new()
            {
                From = new MailAddress(senderMailAddress),
                Subject = "Onaylama Maili",
                Body = bodyResult
            };
            SmtpClient mailServer = new()
            {
                Credentials = new NetworkCredential(senderMailAddress, senderMailPass),
                Port = mailServerPort,
                Host = mailServerHost,
                EnableSsl = mailServerSSL
            };

            message.To.Add(userMailAdress);
            mailServer.Send(message);
        }
    }
}