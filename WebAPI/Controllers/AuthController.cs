using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace WebAPI
{
    [Route("/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody][Required] RegistrationDTO request)
        {
            var result = await _authService.RegisterAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody][Required] LoginDTO request)
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
        public async Task<IActionResult> RefreshToken([FromBody][Required] string refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);
            if (!result.Success) return Unauthorized(result);
            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody][Required] ForgotPasswordDTO request)
        {
            var success = await _authService.ForgotPasswordAsync(request.Email);
            if (!success) return BadRequest(new { message = "Invalid email" });

            return Ok(new { message = "Password reset link sent to your email" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody][Required] ResetPasswordDTO request)
        {
            var success = await _authService.ResetPasswordAsync(request);
            if (!success) return BadRequest(new { message = "Failed to reset password" });

            return Ok(new { message = "Password reset successfully" });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(
            [FromQuery][Required] string userId,
            [FromQuery][Required] string token)
        {
            var success = await _authService.ConfirmEmailAsync(userId, token);
            if (!success) return BadRequest(new { message = "Invalid or expired token" });

            return Ok(new { message = "Email confirmed successfully" });
        }
    }
}
