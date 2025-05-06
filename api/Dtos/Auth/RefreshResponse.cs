namespace api.Dtos.Auth
{
    public class RefreshResponse
    {
        public required string Token { get; set; }
        public int ExpiresIn { get; set; }
        public string? RefreshToken { get; set; }
    }
}