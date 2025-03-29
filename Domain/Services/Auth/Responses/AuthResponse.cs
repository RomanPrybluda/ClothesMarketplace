namespace Domain.Services.Auth.Responses;

public class AuthResponse
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = [];
}
