using api.Models.Auth;

namespace api.Dtos.Auth
{
    public class AuthResponse
    {
        public required string Token { get; set; }
        public required User User { get; set; }
        public required int ExpiresIn { get; set; }
        public required string RefreshToken { get; set; }
    }
}