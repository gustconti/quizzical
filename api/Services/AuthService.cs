using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Dtos.Auth;
using api.Entities.Auth;
using api.Models.Auth;
using api.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class AuthService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration config,
        TokenRepository tokenRepository
    )
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly IConfiguration _config = config;
        private readonly TokenRepository _tokenRepository = tokenRepository;

        public async Task<AuthResponse> LoginAsync(LoginPayload model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email) ?? throw new InvalidOperationException("User not found.");

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Invalid login attempt.");
            }

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, model.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(key)) throw new InvalidOperationException("JWT Key is not configured.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            var refreshToken = Guid.NewGuid().ToString();
            var refreshTokenExpiry = DateTime.Now.AddDays(30);
            RefreshToken refreshTokenEntity = new()
            {
                Token = refreshToken,
                UserId = user.Id,
                Expires = refreshTokenExpiry,
                IsRevoked = false
            };

            await _tokenRepository.SaveTokenAsync(refreshTokenEntity);

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                User = new User
                {
                    Email = model.Email,
                    Id = user.Id,
                    UserName = user.UserName
                },
                ExpiresIn = 3600
            };
        }

        public async Task<RefreshResponse> RefreshTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(key)) throw new InvalidOperationException("JWT Key is not configured.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],
                ValidateLifetime = false // We are only validating the signature here
            };

            try
            {
                // Validate the incoming token
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                var email = principal.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? throw new SecurityTokenException("Invalid token payload");
                var user = await _userManager.FindByEmailAsync(email) ?? throw new InvalidOperationException("User not found.");

                // Check if we need to rotate the refresh token. For now, always generate it.
                bool shouldRotateRefreshToken = true;
                // Generate a new refresh token if rotating, else reuse the old one
                var refreshToken = shouldRotateRefreshToken ? Guid.NewGuid().ToString() : token;
                if (shouldRotateRefreshToken)
                {
                    var refreshTokenEntity = new RefreshToken
                    {
                        UserId = user.Id,
                        Token = refreshToken,
                        Expires = DateTime.Now.AddDays(30) // Set the expiry for the refresh token (e.g., 30 days)
                    };
                    await _tokenRepository.SaveTokenAsync(refreshTokenEntity); // Save the refresh token to the database
                }

                // Generate a new token
                var newClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var newToken = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: newClaims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

                return new RefreshResponse
                {
                    Token = tokenHandler.WriteToken(newToken),
                    ExpiresIn = 3600,
                    RefreshToken = shouldRotateRefreshToken ? refreshToken : null // Include the refresh token only if it's rotated
                };
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Invalid token", ex);
            }
        }

    }
}