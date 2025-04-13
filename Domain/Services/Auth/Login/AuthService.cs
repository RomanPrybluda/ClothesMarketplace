using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Services.Auth.ExtraServices;

namespace Domain;

public class AuthService(
    UserManager<AppUser> _userManager,
    JwtService _jwtService,
    EmailService _emailService)
{
    public async Task<AuthResponse> RegisterAsync(RegistrationDTO request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new AuthResponse { Success = false, Errors = ["Email is already taken."] };
        }

        if (await _userManager.FindByNameAsync(request.UserName) != null)
        {
            return new AuthResponse { Success = false, Errors = ["Username is already taken."] };
        }

        if (request.Password != request.ConfirmPassword)
        {
            return new AuthResponse { Success = false, Errors = ["Passwords do not match."] };
        }

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

        return new AuthResponse { Success = true, Message = "User registered. Please confirm your email." };
    }
    
    public async Task<AuthResponse> LoginAsync(LoginDTO request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return new AuthResponse { Success = false, Message = "Invalid credentials" };
        }

     /*   if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            return new AuthResponse { Success = false, Message = "Email is not confirmed" };
     }  */  

        // Генерація JWT токену та Refresh токену
        var token = _jwtService.GenerateJwtToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        // Збереження Refresh токену в користувача
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new AuthResponse { Success = true, Token = token, RefreshToken = refreshToken };
    }



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

    public async Task<bool> ResetPasswordAsync(ResetPasswordDTO request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        return false;

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return false;

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        return result.Succeeded;
    }

    public async Task<AuthResponse> RefreshTokenAsync(string token)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == token);
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return new AuthResponse { Success = false, Message = "Invalid or expired refresh token" };
        }

        var newJwt = _jwtService.GenerateJwtToken(user);
        var newRefresh = _jwtService.GenerateRefreshToken();
        user.RefreshToken = newRefresh;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userManager.UpdateAsync(user);
        return new AuthResponse { Success = true, Token = newJwt, RefreshToken = newRefresh };
    }

    public async Task LogoutAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return;

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _userManager.UpdateAsync(user);
    }
}
