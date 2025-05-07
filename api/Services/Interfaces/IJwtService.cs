using System.Security.Claims;

namespace api.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string email, IEnumerable<Claim>? additionalClaims = null);
        ClaimsPrincipal? GetPrincipalFromToken(string token);
    }
}