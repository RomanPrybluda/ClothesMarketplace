using Microsoft.AspNetCore.Identity.Data;

namespace Domain.Services.Auth.Login.DTO;

public class RegistrationDto(RegisterRequest register)
{
    public required string UserName { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}
