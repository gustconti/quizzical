using api.Models.Auth;

namespace api.Dtos.Auth
{
    public class AuthResponse
    {
        public required string Jwt { get; set; }
        public required int ExpiresIn { get; set; }
        public required User User { get; set; }
        public required RefreshTokenDto RefreshToken { get; set; }
    }
}