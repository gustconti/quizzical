namespace api.Dtos.Auth
{
    public class RefreshTokenResponse
    {
        public required string Token { get; set; }
        public required string UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
    }
}