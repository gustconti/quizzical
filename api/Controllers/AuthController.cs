using api.Dtos.Auth;
using api.Entities.Auth;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class AuthController(UserManager<ApplicationUser> userManager, IAuthService authService) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterPayload model)
        {
            try
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var loginPayload = new LoginPayload { Email = model.Email, Password = model.Password };
                    return Ok(new { Message = "User registered successfully"});
                }
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginPayload model)
        {
            try
            {
                AuthResponse authResponse = await _authService.LoginAsync(model);

                Response.Cookies.Append("refreshToken", authResponse.RefreshToken.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = authResponse.RefreshToken.Expires
                });

                return Ok(authResponse);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutPayload model)
        {
            try
            {
                await _authService.LogoutAsync(model);
                return Ok(new { Message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh()
        {
            var currentRefreshToken = Request.Cookies["refreshToken"];
            if (currentRefreshToken is null)
                return Unauthorized(new { Message = "Missing refresh token" });

            var jwt = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var refreshResponse = await _authService.RefreshTokenAsync(jwt, currentRefreshToken);

            if (refreshResponse.RotatedRefreshToken != null)
            {
                Response.Cookies.Append("refreshToken", refreshResponse.RotatedRefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = refreshResponse.RefreshToken.Expires
                });
            }

            return Ok(new { token = refreshResponse.Jwt });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound(new { Message = "User not found" });
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Email confirmed successfully" });
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationPayload model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return NotFound(new { Message = "User not found" });
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            // Logic to send email with token
            return Ok(new { Message = "Confirmation email resent successfully" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordPayload model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return NotFound(new { Message = "User not found" });
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // Logic to send email with token
            return Ok(new { Message = "Password reset email sent successfully" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordPayload model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return NotFound(new { Message = "User not found" });
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Password reset successfully" });
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordPayload model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return NotFound(new { Message = "User not found" });
            }
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Password changed successfully" });
            }
            return BadRequest(result.Errors);
        }
    }
}