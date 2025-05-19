namespace api.Dtos.Auth
{
    public class ForgotPasswordPayload
    {
        public string Email { get; set; } = string.Empty;
        public string? Token { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? ConfirmPassword { get; set; } = string.Empty;
    }
}