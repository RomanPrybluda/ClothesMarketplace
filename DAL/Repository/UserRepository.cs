using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class UserRepository(UserManager<AppUser> userManager)
    {
        public async Task<AppUser> FindByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<AppUser> FindByNameAsync(string userName)
        {
            return await userManager.FindByNameAsync(userName);
        }
    }
}
