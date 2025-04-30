using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class UserRepository(UserManager<AppUser> userManager)
    {
        public async Task<List<AppUser>> GetUsersAsync(int page, int pageSize)
        {
            var query = userManager.Users.Select(u => new AppUser
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            });

            var users = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return users;
        }

        public async Task<AppUser> FindByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<AppUser> FindByNameAsync(string userName)
        {
            return await userManager.FindByNameAsync(userName);
        }

        public async Task<AppUser> FindByIdAsync(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateUserAsync(AppUser user)
        {
            return await userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = await FindByIdAsync(userId);

            if (user != null)
                return await userManager.DeleteAsync(user);

            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> AddToRoleAsync(AppUser user, string roleName)
        {
            return await userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<int> GetUserCount()
        {
            return await userManager.Users.CountAsync();
        }
    }
}
