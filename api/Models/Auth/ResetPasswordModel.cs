namespace api.Models.Auth
{
    public class ResetPasswordModel
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
        public required string Password { get; set; }
        public string NewPassword { get; set; } = string.Empty;
    }
}