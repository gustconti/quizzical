namespace api.Services.Interfaces
{
    using api.Entities.Auth;
    public interface IRefreshTokenService
    {
        Task<RefreshToken> GenerateRefreshTokenAsync(string userId);
        Task<RefreshToken> GetAndValidateAsync(string token, string userId);
        bool ShouldRotate(RefreshToken token);
    }
}