namespace Apollo.Entities 
{
    public class Team : BaseEntity
    {
        public string TeamName { get; set; }
        public string MailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ProfilePhotoPath { get; set; }
        public bool IsMailVerified { get; set; }
        public bool IsPhoneNumberVerified { get; set; }
    }
}