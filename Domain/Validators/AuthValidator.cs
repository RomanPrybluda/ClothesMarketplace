using DAL.Repository;
using Domain.Сommon.Wrappers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validators
{
    public class AuthValidator(UserRepository userRepository)
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
    }
}
