using DAL;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Domain
{
    public class AdminInitializer
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private const string AdminEmail = "admin@example.com";
        private const string AdminUserName = "admin";
        private const string AdminPassword = "Admin123!";

        public AdminInitializer(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAdmin()
        {
            var admin = await _userManager.FindByEmailAsync(AdminEmail);
            if (admin != null)
                return; 

            var newAdmin = new AppUser
            {
                UserName = AdminUserName,
                Email = AdminEmail,
                FirstName = "Admin",
                LastName = "User",
                Age = 30,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(newAdmin, AdminPassword);
            if (!createResult.Succeeded)
                return; 

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            await _userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }
}
