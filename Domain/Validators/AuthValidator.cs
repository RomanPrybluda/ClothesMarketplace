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
        public Task<bool> ValidateRegistrationDto(RegistrationDTO request)
        {
            var existingUser = await userRepository.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResponse { Success = false, Errors = ["Email is already taken."] };
            }

            if (await _userManager.FindByNameAsync(request.UserName) != null)
            {
                return new AuthResponse { Success = false, Errors = ["Username is already taken."] };
            }

            if (request.Password != request.ConfirmPassword)
            {
                return new AuthResponse { Success = false, Errors = ["Passwords do not match."] };
            }
        }
    }
}
