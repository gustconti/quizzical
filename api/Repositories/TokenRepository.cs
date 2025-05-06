using api.Data;
using api.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class TokenRepository(ApplicationDbContext context)
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
            if (existing != null)
            {
                existing.IsRevoked = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UseTokenAsync(string token)
        {
            var existing = await GetTokenAsync(token);
            if (existing != null)
            {
                existing.IsUsed = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
