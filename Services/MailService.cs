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
        public void PlayerWelcomeMail(string playerMailAddress)
        {
            MailMessage message = new MailMessage()
            {
                From = new MailAddress(senderMailAddress),
                Subject = "Welcome / Hoşgeldiniz",
                Body = "Deneme"
            };
            SmtpClient mailServer = new SmtpClient()
            {
                Credentials = new System.Net.NetworkCredential(senderMailAddress , senderMailPass),
                Port = mailServerPort,
                Host = mailServerHost,
                EnableSsl = mailServerSSL
            };

            message.To.Add(playerMailAddress);
            mailServer.Send(message);
        }
        public void TeamWelcomeMail(string teamMailAddress)
        {
            MailMessage message = new MailMessage()
            {
                From = new MailAddress(senderMailAddress),
                Subject = "Welcome / Hoşgeldiniz",
                Body = "Deneme mailidir."
            };
            SmtpClient mailServer = new SmtpClient()
            {
                Credentials = new System.Net.NetworkCredential(senderMailAddress , senderMailPass),
                Port = mailServerPort,
                Host = mailServerHost,
                EnableSsl = mailServerSSL
            };
            
            message.To.Add(teamMailAddress);
            mailServer.Send(message);
        }
    }
}