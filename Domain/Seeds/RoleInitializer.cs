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
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "B31C4D4F-8C8B-44B5-94A7-30B6947E9C93",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "ED292C9E-0A58-4A17-AE91-68986B0B3AC8",
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

            foreach (var role in roles)
            {
                var roleExists = _roleManager.RoleExistsAsync(role.Name).Result;

                if (!roleExists)
                {
                    var createResult = _roleManager.CreateAsync(role).Result;
                    if (!createResult.Succeeded)
                    {
                        throw new InvalidOperationException($"Failed to create role {role.Name}");
                    }
                }
            }
        }
    }
}
