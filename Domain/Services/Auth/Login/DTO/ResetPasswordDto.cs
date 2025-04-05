namespace Domain.Services.Auth.Login.DTO;

public class ResetPasswordDTO
{
    public string? Email { get; set; }
    public string? Token { get; set; }
    public string? NewPassword { get; set; }
}
