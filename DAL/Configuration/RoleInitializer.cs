using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class RoleInitializer
    {
        public static async Task Initialize(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var roleNames = new[] { "Admin", "Seller", "Buyer" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User"
                };
                await userManager.CreateAsync(adminUser, "Admin123!");

                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            var sellerUser = await userManager.FindByEmailAsync("seller@example.com");
            if (sellerUser == null)
            {
                sellerUser = new AppUser
                {
                    UserName = "seller@example.com",
                    Email = "seller@example.com",
                    FirstName = "Seller",
                    LastName = "User"
                };
                await userManager.CreateAsync(sellerUser, "Seller123!");

                await userManager.AddToRoleAsync(sellerUser, "Seller");
            }

            var buyerUser = await userManager.FindByEmailAsync("buyer@example.com");
            if (buyerUser == null)
            {
                buyerUser = new AppUser
                {
                    UserName = "buyer@example.com",
                    Email = "buyer@example.com",
                    FirstName = "Buyer",
                    LastName = "User"
                };
                await userManager.CreateAsync(buyerUser, "Buyer123!");

                await userManager.AddToRoleAsync(buyerUser, "Buyer");
            }
        }
    }
}
