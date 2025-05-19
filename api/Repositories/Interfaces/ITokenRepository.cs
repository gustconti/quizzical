using api.Entities.Auth;

namespace api.Repositories.Interfaces
{
    public interface ITokenRepository 
    {
        public Task SaveTokenAsync(RefreshToken token);
        public Task<RefreshToken?> GetTokenAsync(string token);
        public Task RevokeTokenAsync(string token);
        public Task UseTokenAsync(string token);
    }
}