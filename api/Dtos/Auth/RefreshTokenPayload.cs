namespace api.Dtos.Auth
{
    public class RefreshTokenPayload
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}