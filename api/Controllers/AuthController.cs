using api.Dtos.Auth;
using api.Entities.Auth;
using api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class AuthController(
        UserManager<ApplicationUser> userManager,
        AuthService authService
    ) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly AuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterPayload model)
        {
            var user = new ApplicationUser{ UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully" });
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginPayload model)
        {
            try
            {
                var authResponse = await _authService.LoginAsync(model);
                return Ok(authResponse);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenPayload model)
        {
            try
            {
                RefreshResponse refreshResponse = await _authService.RefreshTokenAsync(model);
                return Ok(refreshResponse);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
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