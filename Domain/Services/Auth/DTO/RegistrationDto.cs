using System.ComponentModel.DataAnnotations;

namespace Domain;

public class RegistrationDTO
{
    public string UserName { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }

    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}
