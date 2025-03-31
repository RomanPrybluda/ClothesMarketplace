using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AppUserService
{
    private readonly UserManager<AppUser> _userManager;

    public AppUserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<AppUser>> GetAllUsersAsync() => await _userManager.Users.ToListAsync();

    public async Task<AppUser?> GetUserByIdAsync(string id) => await _userManager.FindByIdAsync(id);

    public async Task<bool> CreateUserAsync(AppUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        return result.Succeeded;
    }

    public async Task<bool> UpdateUserAsync(AppUser user)
    {
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
        return false;
    }
}
