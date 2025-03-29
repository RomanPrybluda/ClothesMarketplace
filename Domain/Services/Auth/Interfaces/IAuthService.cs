using Domain.Services.Auth.Login.DTO;
using Domain.Services.Auth.Responses;

namespace Domain.Services.Auth.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegistrationDto request);

    Task<AuthResponse> LoginAsync(LoginDto request);

    Task LogoutAsync(string userId);

    Task<bool> ConfirmEmailAsync(string userId, string token);

    Task<bool> ForgotPasswordAsync(string email);

    Task<bool> ResetPasswordAsync(ResetPasswordDto request);

    Task<AuthResponse> RefreshTokenAsync(string token);
}
