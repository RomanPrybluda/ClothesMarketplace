using DAL.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validators
{
    public class AuthValidator(UserRepository userRepository)
    {
        public async Task<bool> ValidateRegistrationDto(RegistrationDTO request)
        {
            var existingUser = await userRepository.FindByEmailAsync(request.Email);
            
            if (existingUser != null)
                throw new CustomException(CustomExceptionType.IsAlreadyExists, "Email is already taken");

            if (await userRepository.FindByNameAsync(request.UserName) != null)
                throw new CustomException(CustomExceptionType.IsAlreadyExists, "Username is already taken.");

            if (request.Password != request.ConfirmPassword)
                throw new CustomException(CustomExceptionType.PasswordsDoNotMatch, "Passwords do not match.");

            return true;
        }
    }
}
