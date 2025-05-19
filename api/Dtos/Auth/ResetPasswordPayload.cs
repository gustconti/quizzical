namespace api.Dtos.Auth
{
    public class ResetPasswordPayload
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
        public required string Password { get; set; }
        public string NewPassword { get; set; } = string.Empty;
    }
}