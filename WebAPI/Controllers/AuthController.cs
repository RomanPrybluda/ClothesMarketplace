using Domain.Services.Auth.Interfaces;
using Domain.Services.Auth.Login.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService _authService): ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto request)
        {
            var result = await _authService.RegisterAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (!result.Success) return Unauthorized(result);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (userId == null) return Unauthorized();

            await _authService.LogoutAsync(userId);
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);
            if (!result.Success) return Unauthorized(result);
            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            var success = await _authService.ForgotPasswordAsync(request.Email);
            if (!success) return BadRequest(new { message = "Invalid email" });

            return Ok(new { message = "Password reset link sent to your email" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            var success = await _authService.ResetPasswordAsync(request);
            if (!success) return BadRequest(new { message = "Failed to reset password" });

            return Ok(new { message = "Password reset successfully" });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var success = await _authService.ConfirmEmailAsync(userId, token);
            if (!success) return BadRequest(new { message = "Invalid or expired token" });

            return Ok(new { message = "Email confirmed successfully" });
        }
    }
}
