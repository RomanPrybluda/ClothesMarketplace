using DAL;
using Domain.Services.Auth.Interfaces;
using Domain.Services.Auth.Login.DTO;
using Domain.Services.Auth.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Domain.Services.Auth.Login;

public class AuthService(UserManager<AppUser> _userManager, 
    SignInManager<AppUser> _signInManager, IJwtService _jwtService, 
    IConfiguration _configuration, IEmailService _emailService) : IAuthService
{
    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task<bool> ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = $"https://your-app.com/reset-password?email={email}&token={Uri.EscapeDataString(token)}";

        await _emailService.SendEmailAsync(user.Email, "Reset Password", $"Click here to reset your password: {resetLink}");
        return true;
    }

    public async Task<AuthResponse> LoginAsync(LoginDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return new AuthResponse { Success = false, Message = "Invalid credentials" };
        }

        var token = _jwtService.GenerateJwtToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userManager.UpdateAsync(user);

        return new AuthResponse { Success = true, Token = token, RefreshToken = refreshToken };
    }

    public Task LogoutAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<AuthResponse> RefreshTokenAsync(string token)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == token);
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return new AuthResponse { Success = false, Message = "Invalid or expired refresh token" };
        }

        var newJwtToken = _jwtService.GenerateJwtToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userManager.UpdateAsync(user);

        return new AuthResponse { Success = true, Token = newJwtToken, RefreshToken = newRefreshToken };
    }

    public async Task<AuthResponse> RegisterAsync(RegistrationDto request)
    {
        var user = new AppUser
        {
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return new AuthResponse { Success = false, Errors = result.Errors.Select(e => e.Description).ToList() };
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = $"https://your-app.com/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

        await _emailService.SendEmailAsync(user.Email, "Confirm your email", $"Click here to confirm your email: {confirmationLink}");

        return new AuthResponse 
        { Success = true, Message = "User registered successfully. Please confirm your email." };
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return false;

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        return result.Succeeded;
    }
}
