using System.Net.Mail;

namespace Apollo.Services
{
    public class MailService
    {
        private string senderMailAddress = "softwareionicc@hotmail.com";
        private string senderMailPass = "kekobabam123";
        private int mailServerPort = 587;
        private string mailServerHost = "smtp.live.com";
        private bool mailServerSSL = true;
        public void playerWelcomeMail(string playerMailAddress)
        {
            MailMessage message = new MailMessage();
            SmtpClient mailServer = new SmtpClient();
            mailServer.Credentials = new System.Net.NetworkCredential(senderMailAddress , senderMailPass);
            mailServer.Port = mailServerPort;
            mailServer.Host = mailServerHost; 
            mailServer.EnableSsl= mailServerSSL;
            message.To.Add(playerMailAddress);
            message.From = new MailAddress("softwareionicc@hotmail.com");
            message.Subject = "Welcome/Hoşgeldiniz";
            message.Body = "deneme";
            mailServer.Send(message);
        }
        public void teamWelcomeMail(string teamMailAddress)
        {
            MailMessage message = new MailMessage();
            SmtpClient mailServer = new SmtpClient();
            mailServer.Credentials = new System.Net.NetworkCredential(senderMailAddress , senderMailPass);
            mailServer.Port = mailServerPort;
            mailServer.Host = mailServerHost; 
            mailServer.EnableSsl= mailServerSSL;
            message.To.Add(teamMailAddress);
            message.From = new MailAddress("softwareionicc@hotmail.com");
            message.Subject = "Takım olarak hoşgeliniz";
            message.Body = "göt oğuz";
            mailServer.Send(message);
        }
    }
}