using DAL;
using DAL.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Domain;

public class JwtService
{
    private readonly JwtTokenOptions _tokenOptions;

    public JwtService(IOptions<JwtTokenOptions> tokenOptions)
    {
        _tokenOptions = tokenOptions.Value;
    }

    public string GenerateJwtToken(AppUser user, params string[] userRoles)
    {
        foreach (var role in userRoles)
            if (!RoleRegistry.IsValidRole(role))
                throw new CustomException(CustomExceptionType.InvalidData, "User role is invalid.");

        if (user == null)
            throw new CustomException(CustomExceptionType.InvalidData, "User cannot be null.");

        var claims = GetClaims(user, userRoles);
        var credentials = GetSigningCredentials();

        var token = new JwtSecurityToken(
            _tokenOptions.Issuer,
            _tokenOptions.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var bytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }

    private SigningCredentials GetSigningCredentials()
    {
        if (string.IsNullOrEmpty(_tokenOptions.Key) || Encoding.UTF8.GetByteCount(_tokenOptions.Key) < 32)
            throw new ArgumentException("JWT Secret Key must be at least 32 bytes long.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.Key));
        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    private List<Claim> GetClaims(AppUser user, params string[] userRoles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}
