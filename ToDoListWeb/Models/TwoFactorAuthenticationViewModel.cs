namespace ToDoListWeb.Models
{
    public class TwoFactorAuthenticationViewModel
    {
        // used to login
        public string Code { get; set; }
        // used to register / sign up
        public string Token { get; set; }
        public string QRCodeUrl { get; set; }
    }
}
