using Apollo.Enums;

namespace Apollo.DTO
{
    public class JwtClaimDTO 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MailAddress { get; set; }
        public UserType UserType { get; set; }
    }
}