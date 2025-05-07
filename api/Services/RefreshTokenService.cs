using api.Entities.Auth;
using api.Options;
using api.Repositories.Interfaces;
using api.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class RefreshTokenService(ITokenRepository tokenRepository, IOptions<RefreshTokenOptions> options) : IRefreshTokenService
    {
        private readonly ITokenRepository _tokenRepository = tokenRepository;
        private readonly RefreshTokenOptions _options = options.Value;

        public async Task<RefreshToken> GenerateRefreshTokenAsync(string userId)
        {
            var token = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userId,
                Expires = DateTime.UtcNow.AddDays(_options.DaysToExpire)
            };

            await _tokenRepository.SaveTokenAsync(token);
            return token;
        }

        public async Task<RefreshToken> GetAndValidateAsync(string token, string userId)
        {
            var refreshToken = await _tokenRepository.GetTokenAsync(token)
                ?? throw new SecurityTokenException("Invalid refresh token.");

            if (refreshToken.UsedAt is not null)
                throw new SecurityTokenException("Refresh token has already been used.");
            if (refreshToken.RevokedAt is not null)
                throw new SecurityTokenException("Refresh token has been revoked.");
            if (refreshToken.Expires < DateTime.UtcNow)
                throw new SecurityTokenException("Refresh token expired.");
            if (refreshToken.UserId != userId)
                throw new SecurityTokenException("Refresh token does not match user.");

            return refreshToken;
        }

        public bool ShouldRotate(RefreshToken token)
        {
            var threshold = TimeSpan.FromDays(_options.DaysToRefresh);
            return token.Expires - DateTime.UtcNow <= threshold;
        }
    }
}