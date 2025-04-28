using AutoMapper;
using DAL;
using DAL.Models;
using Domain.Services.Auth.DTO;
using Domain.Validators;
using Domain.Сommon.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public class AuthService(
    UserManager<AppUser> _userManager,
    JwtService _jwtService,
    EmailService _emailService,
    AuthValidator authValidator,
    IMapper mapper,
    AppUserService _userService)
{

    public async Task<Result<RegistrationResponseDTO>> RegisterAsync(RegistrationDTO request)
    {
        var validationResult = await authValidator.ValidateRegistrationDto(request);

        if(!validationResult.IsValid)
            return Result<RegistrationResponseDTO>.Failure(validationResult.GetExceptionsList());

        var user = mapper.Map<AppUser>(request);
        await _userService.CreateUserAsync(user, request.Password, RoleRegistry.User);

        user.RefreshToken = _jwtService.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        var token = _jwtService.GenerateJwtToken(user);

        await _userService.UpdateUserAsync(user.Id, user);

        return Result<RegistrationResponseDTO>.Success(new RegistrationResponseDTO
        {
            Token = token,
            RefreshToken = user.RefreshToken,
            Message = "User registered successfully."
        });
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

        var token = _jwtService.GenerateJwtToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

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
