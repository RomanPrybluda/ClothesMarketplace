using DAL;
using DAL.Repository;
using Domain.Сommon.Wrappers;
using Microsoft.AspNetCore.Identity;

namespace Domain.Validators
{
    public class AuthValidator(UserRepository userRepository, UserManager<AppUser> userManager)
    {
        public async Task<ValidationSummary> ValidateRegistrationDto(RegistrationDTO request)
        {
            var validationResult = new List<Exception>();
            var existingUser = await userRepository.FindByEmailAsync(request.Email);

            if (existingUser != null)
                validationResult.Add(new CustomException(CustomExceptionType.IsAlreadyExists, "Email is already taken"));

            if (await userRepository.FindByNameAsync(request.UserName) != null)
                validationResult.Add(new CustomException(CustomExceptionType.IsAlreadyExists, "Username is already taken."));

            if (request.Password != request.ConfirmPassword)
                validationResult.Add(new CustomException(CustomExceptionType.PasswordsDoNotMatch, "Passwords do not match."));

            return new ValidationSummary(validationResult);
        }

        public async Task<ValidationSummary> ValidateLoginDto(LoginDTO request)
        {
            var validationResult = new List<Exception>();
            var existingUser = await userRepository.FindByEmailAsync(request.Email);

            if (existingUser == null)
            {
                validationResult.Add(new CustomException(CustomExceptionType.NotFound, $"No user found with email {request.Email}"));
                return new ValidationSummary(validationResult);
            }

            var isValidPassword = await userManager.CheckPasswordAsync(existingUser, request.Password);

            if (!isValidPassword)
                validationResult.Add(new CustomException(CustomExceptionType.InvalidPassword, "Invalid password"));

            return new ValidationSummary(validationResult);
        }
    }
}
