namespace Apollo.ViewModels
{
    public class UserResetPasswordViewModel
    {
        public string Name { get; set; }
        public int ConfirmationCode { get; set; }
        public string Link { get; set; }
        public string Password { get; set; }
    }
}