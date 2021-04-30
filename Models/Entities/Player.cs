using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apollo.Entities 
{
    public class Player : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }    
        public string PhoneNumber { get; set;}
        public string MailAddress { get; set;}
        public string Password { get; set; }
        public string CVDescription { get; set; }
        public string TwitterContact { get; set; }
        public string FacebookContact { get; set; }
        public string YoutubeContact { get; set; }
        public int ProfilePhotoId { get; set; }
        public int SalaryException { get; set; }
        [ForeignKey("CityId")]
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public bool IsActiveForTeam { get; set; }
        public bool IsVerifiedPlayer { get; set; }
        public bool IsMailVerified { get; set; }
        public bool IsPhoneNumberVerified { get; set; }
        public DateTime BirtDate { get; set; }
    }
}