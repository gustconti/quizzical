using api.Data;
using api.Entities.Auth;
using api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class TokenRepository(ApplicationDbContext context) : ITokenRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task SaveTokenAsync(RefreshToken token)
        {
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task RevokeTokenAsync(string token)
        {
            var existing = await GetTokenAsync(token);
            if (existing is not null)
            {
                existing.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UseTokenAsync(string token)
        {
            var existing = await GetTokenAsync(token);
            if (existing is not null)
            {
                existing.UsedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
