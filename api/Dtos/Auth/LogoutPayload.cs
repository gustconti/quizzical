namespace api.Dtos.Auth
{
    public class LogoutPayload
    {
        public required string UserId { get; set; }
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}