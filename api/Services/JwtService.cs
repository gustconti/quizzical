using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Options;
using api.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class JwtService(IOptions<JwtOptions> options) : IJwtService
    {
        private readonly JwtOptions _options = options.Value;
        private readonly SymmetricSecurityKey _key = new(Encoding.UTF8.GetBytes(options.Value.Key));

        public string GenerateToken(string email, IEnumerable<Claim>? additionalClaims = null)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (additionalClaims != null)
                claims.AddRange(additionalClaims);

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateIssuer = true,
                ValidIssuer = _options.Issuer,
                ValidateAudience = true,
                ValidAudience = _options.Audience,
                ValidateLifetime = false // we check expiry manually for refresh
            };

            return tokenHandler.ValidateToken(token, parameters, out _);
        }
    }
}