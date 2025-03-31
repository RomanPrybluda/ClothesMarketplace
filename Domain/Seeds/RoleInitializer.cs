using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DAL
{
    public class RoleInitializer
    {
        public static async Task InitializeRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User" };
            
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}