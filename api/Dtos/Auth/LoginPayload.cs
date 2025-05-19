namespace api.Dtos.Auth
{
    public class LoginPayload
    {
        public required string Email { get; set; }
        public string? UserName { get; set; }
        public required string Password { get; set; }
        public bool RememberMe { get; internal set; } = true;
    }
}