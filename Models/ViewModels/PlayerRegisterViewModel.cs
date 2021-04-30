using System;

namespace Apollo.ViewModelds
{
    public class PlayerRegisterViewModel 
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }
        public string PhoneNumber { get; set; }
        public string MailAddress { get; set; }
        public string Password { get; set; }
        public string PasswordVerify { get; set; }
        public int CityId { get; set; }
        public DateTime BirthDate { get; set; }
    }
}