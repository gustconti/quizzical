namespace api.Dtos.Auth
{
    public class RefreshTokenDto
    {
        public required string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}