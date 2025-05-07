using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api.Dtos.Auth;
using api.Entities.Auth;
using api.Models.Auth;
using api.Options;
using api.Repositories.Interfaces;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class AuthService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ITokenRepository tokenRepository,
        IJwtService jwtService,
        IRefreshTokenService refreshTokenService,
        IOptions<JwtOptions> options
    )
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly ITokenRepository _tokenRepository = tokenRepository;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;
        private readonly JwtOptions _jwtOptions = options.Value;
        public async Task<AuthResponse> LoginAsync(LoginPayload model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email) ?? throw new InvalidOperationException("User not found.");
            if (!user.EmailConfirmed) throw new InvalidOperationException("Email not confirmed.");

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (!result.Succeeded) throw new InvalidOperationException("Invalid login attempt.");

            var jwtToken = _jwtService.GenerateToken(model.Email);
            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);

            return new AuthResponse
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                User = new User
                {
                    Email = model.Email,
                    Id = user.Id,
                    UserName = user.UserName
                },
                ExpiresIn = _jwtOptions.ExpiresInSeconds
            };
        }
        public async Task<RefreshResponse> RefreshTokenAsync(RefreshTokenPayload model)
        {
            var principal = _jwtService.GetPrincipalFromToken(model.Token);

            string email = principal?.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? throw new SecurityTokenException("Invalid email claim in token.");
            IdentityUser user = await _userManager.FindByEmailAsync(email)
                ?? throw new InvalidOperationException("User not found");

            RefreshToken currentRefreshToken = await _refreshTokenService.GetAndValidateAsync(model.RefreshToken, user.Id);
            string refreshTokenToReturn = model.RefreshToken;

            bool shouldRotate = _refreshTokenService.ShouldRotate(currentRefreshToken);

            if (shouldRotate)
            {
                await _tokenRepository.UseTokenAsync(currentRefreshToken.Token);
                var newRefreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);
                refreshTokenToReturn = newRefreshToken.Token;
            }

            var newJwt = _jwtService.GenerateToken(email);

            return new RefreshResponse
            {
                Token = newJwt,
                ExpiresIn = _jwtOptions.ExpiresInSeconds,
                RefreshToken = refreshTokenToReturn,
            };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}