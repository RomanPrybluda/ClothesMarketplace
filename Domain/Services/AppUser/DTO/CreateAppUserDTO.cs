using DAL;

namespace Domain;
public class CreateAppUserDTO
{
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    public static AppUser ToAppUser(CreateAppUserDTO request)
    {     
        return new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = request.UserName ?? string.Empty,
            Email = request.Email ?? string.Empty,
            FirstName = request.FirstName ?? string.Empty,
            LastName = request.LastName ?? string.Empty,
        };
    }
}
        