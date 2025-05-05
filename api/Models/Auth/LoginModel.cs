namespace api.Models.Auth
{
    public class LoginModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool RememberMe { get; internal set; }
    }
}