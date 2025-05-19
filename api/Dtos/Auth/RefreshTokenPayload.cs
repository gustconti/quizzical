namespace api.Dtos.Auth
{
    public class RefreshTokenPayload
    {
        public required string Jwt { get; set; }
        public required string RefreshToken { get; set; }
    }
}