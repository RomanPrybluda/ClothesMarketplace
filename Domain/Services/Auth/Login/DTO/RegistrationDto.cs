
namespace Domain.Services.Auth.Login.DTO;

public class RegistrationDTO
{
    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? ConfirmPassword { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}
