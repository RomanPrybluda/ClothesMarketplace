using DAL;
using DAL.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class LoginDtoValidator : AbstractValidator<LoginDTO>
    {
        public LoginDtoValidator(UserRepository userRepository, UserManager<AppUser> userManager)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Field email is required.")
                .MustAsync(async (email, cancellation) =>
                {
                    var existingUser = await userRepository.FindByEmailAsync(email);
                    return existingUser != null;
                })
                .WithMessage("No user found with this email address. Please check the email and try again.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MustAsync(async (password, cancellation) =>
                {
                    var user = await userManager.FindByEmailAsync(password);
                    var isValid = await userManager.CheckPasswordAsync(user, password);
                    return !isValid;
                })
                .WithMessage("Invalid password. Please check the password and try again")
                .WhenAsync(async (x, cancellationTokjen) => await userRepository.FindByEmailAsync(x.Email) != null);
        }
    }
}
