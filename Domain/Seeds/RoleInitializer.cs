using Microsoft.AspNetCore.Identity;

namespace DAL
{
    public class RoleInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleInitializer(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public void InitializeRoles()
        {
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                var roleExist = _roleManager.RoleExistsAsync(roleName).Result;

                if (!roleExist)
                {
                    var createResult = _roleManager.CreateAsync(new IdentityRole(roleName)).Result;
                    if (!createResult.Succeeded)
                    {
                        throw new InvalidOperationException($"Fail to create role {roleName}.");
                    }
                }
            }
        }
    }
}
