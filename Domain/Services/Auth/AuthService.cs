using AutoMapper;
using DAL;
using DAL.Models;
using Domain.Сommon.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public class AuthService(
    UserManager<AppUser> _userManager,
    JwtService _jwtService,
    EmailService _emailService,
    IValidator<LoginDTO> loginValidator,
    IMapper mapper,
    AppUserService _userService,
    IValidator<RegistrationDTO> registrationValidator)
{

    public async Task<Result<RegistrationResponseDTO>> RegisterAsync(RegistrationDTO request, IdentityRole userRole)
    {
        var validationResult = await registrationValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
            return Result<RegistrationResponseDTO>.Failure(validationResult.Errors);

        var user = mapper.Map<AppUser>(request);
        user.RefreshToken = _jwtService.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        var userCreationResult = await _userService.CreateUserAsync(user, request.Password, userRole);

        if (!userCreationResult.IsSuccess)
            return Result<RegistrationResponseDTO>.Failure(userCreationResult.Errors);

        var token = _jwtService.GenerateJwtToken(user, RoleRegistry.User.Name);

        return Result<RegistrationResponseDTO>.Success(new RegistrationResponseDTO
        {
            Token = token,
            RefreshToken = user.RefreshToken,
            Message = "User registered successfully."
        });
    }

    public async Task<Result<LoginResponseDTO>> LoginAsync(LoginDTO request)
    {
        var validationResult = await loginValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
            return Result<LoginResponseDTO>.Failure(validationResult.Errors);

        var user = await _userManager.FindByEmailAsync(request.Email);
        var userRoles = await _userManager.GetRolesAsync(user);
        var token = _jwtService.GenerateJwtToken(user, userRoles.ToArray());
        var refreshToken = _jwtService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userManager.UpdateAsync(user);

        return Result<LoginResponseDTO>.Success(new LoginResponseDTO
        {
            Success = true,
            Token = token,
            RefreshToken = refreshToken,
            Message = "Signed in successfully."
        });
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

        var newJwt = _jwtService.GenerateJwtToken(user, RoleRegistry.User.Name);
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
