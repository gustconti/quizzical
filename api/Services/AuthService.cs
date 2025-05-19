using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api.Dtos.Auth;
using api.Entities.Auth;
using api.Models.Auth;
using api.Options;
using api.Repositories.Interfaces;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenRepository tokenRepository,
        IJwtService jwtService,
        IRefreshTokenService refreshTokenService,
        IOptions<JwtOptions> options
    ) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly ITokenRepository _tokenRepository = tokenRepository;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;
        private readonly JwtOptions _jwtOptions = options.Value;
        public async Task<AuthResponse> LoginAsync(LoginPayload model)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(model.Email) ?? throw new InvalidOperationException("User not found.");
            // if (!user.EmailConfirmed) throw new InvalidOperationException("Email not confirmed.");

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (!result.Succeeded) throw new InvalidOperationException("Invalid login attempt.");
            if (result.IsLockedOut) throw new InvalidOperationException("User is locked out.");

            var jwtToken = _jwtService.GenerateToken(user.Id);
            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);
            var RefreshTokenDto = new RefreshTokenDto
            {
                Token = refreshToken.Token,
                Expires = refreshToken.Expires
            };
            
            return new AuthResponse
            {
                Jwt = jwtToken,
                User = new User
                {
                    Email = model.Email,
                    Id = user.Id,
                    UserName = user.UserName
                },
                ExpiresIn = _jwtOptions.ExpiresInSeconds,
                RefreshToken = RefreshTokenDto,
            };
        }

        public async Task LogoutAsync(LogoutPayload model)
        {
            await _tokenRepository.RevokeTokenAsync(model.RefreshToken);
        }

        public async Task<RefreshResponse> RefreshTokenAsync(string jwt, string refreshToken)
        {
            var principal = _jwtService.GetPrincipalFromToken(jwt);

            string email = principal?.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? throw new SecurityTokenException("Invalid email claim in token.");
            IdentityUser user = await _userManager.FindByEmailAsync(email)
                ?? throw new InvalidOperationException("User not found");

            RefreshToken currentRefreshToken = await _refreshTokenService.GetAndValidateAsync(refreshToken, user.Id);

            RefreshToken RefreshTokenToReturn = currentRefreshToken;
            bool shouldRotate = _refreshTokenService.ShouldRotate(currentRefreshToken);

            if (shouldRotate)
            {
                await _tokenRepository.UseTokenAsync(currentRefreshToken.Token);
                RefreshTokenToReturn = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);
            }

            var newJwt = _jwtService.GenerateToken(email);

            return new RefreshResponse
            {
                Jwt = newJwt,
                ExpiresIn = _jwtOptions.ExpiresInSeconds,
                RefreshToken = RefreshTokenToReturn,
            };
        }
    }
}