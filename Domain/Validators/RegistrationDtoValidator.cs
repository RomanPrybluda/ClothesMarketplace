using DAL.Repository;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validators
{
    public class RegistrationDtoValidator : AbstractValidator<RegistrationDTO>
    {
        public RegistrationDtoValidator(UserRepository userRepository)
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("User name is required.")
                .MaximumLength(30)
                .WithMessage("User name must be less than 30 characters.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .Matches("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")
                .WithMessage("Invalid email address.")
                .MustAsync(async (email, cancellation) =>
                {
                    var user = await userRepository.FindByEmailAsync(email);
                    return user == null;
                })
                .WithMessage("An account with this email already exists.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(8)
                .WithMessage("The password field should be at least 8 and maximum 20 characters.")
                .MaximumLength(256);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Confirm password is required")
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required")
                .MaximumLength(15)
                .WithMessage("First name must be less than 15 characters");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required")
                .MaximumLength(30)
                .WithMessage("Last name must be less than 30 characters");
        }
    }
    
}
