using System.Net;
using System.Net.Mail;
using Apollo.Data;
using Apollo.Entities;
using Apollo.Enums;

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
            MailMessage message = new()
            {
                From = new MailAddress(senderMailAddress),
                Subject = "Welcome / Hoşgeldiniz",
                Body = "Deneme"
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
        public void TeamWelcomeMail(string teamMailAddress)
        {
            MailMessage message = new()
            {
                From = new MailAddress(senderMailAddress),
                Subject = "Welcome / Hoşgeldiniz",
                Body = "Deneme mailidir."
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

        public void UserSendMailVerification(int confirmationCode, string url, string userMailAdress)
        {
            MailMessage message = new()
            {
                From = new MailAddress(senderMailAddress),
                Subject = "Onaylama Maili",
                Body = string.Format("<a href='{0}'>Link'e git.</a> Onaylama Kodunuz: {1}", url, confirmationCode)
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