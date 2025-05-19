using api.Entities.Auth;

namespace api.Dtos.Auth
{
    public class RefreshResponse
    {
        public required string Jwt { get; set; }
        public required int ExpiresIn { get; set; }
        public string? RotatedRefreshToken { get; set; }
        public required RefreshToken RefreshToken { get; set; }
    }
}