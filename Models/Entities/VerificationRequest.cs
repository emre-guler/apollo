using Apollo.Enums;

namespace Apollo.Entities
{
    public class VerificationRequest : BaseEntity
    {
        public UserType UserType { get; set; }
        public int UserId { get; set; }
        public string URL { get; set; }
        public int ConfirmationCode { get; set; }
    }
}