using DAL;

namespace Domain.Services.Auth.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(AppUser user);
    string GenerateRefreshToken();
}
