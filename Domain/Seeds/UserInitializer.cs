using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DAL
{
    public class UserInitializer
    {
        public static async Task InitializeUsers(UserManager<AppUser> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@example.com") == null)
            {
                var admin = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            if (await userManager.FindByEmailAsync("user@example.com") == null)
            {
                var user = new AppUser
                {
                    UserName = "user",
                    Email = "user@example.com",
                    FirstName = "Name",
                    LastName = "Surname",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "User123!");
                await userManager.AddToRoleAsync(user, "User");
            }
        }
    }
}