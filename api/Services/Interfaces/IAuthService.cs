using System.Security.Claims;
using api.Dtos.Auth;

namespace api.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<AuthResponse> LoginAsync(LoginPayload model);
        public Task<RefreshResponse> RefreshTokenAsync(string jwt, string refreshToken);
        public Task LogoutAsync(LogoutPayload model);
    }
}