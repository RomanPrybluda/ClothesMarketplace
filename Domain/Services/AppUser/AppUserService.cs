using DAL;
using Domain;
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

    public async Task<PagedResult<AppUserDTO>> GetAllUsersAsync(int page, int pageSize)
    {
        var query = _userManager.Users.Select(u => new AppUserDTO
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email
        });

        var totalUsers = await query.CountAsync();
        var users = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<AppUserDTO> { Items = users, TotalCount = totalUsers };
    }

    public async Task<AppUserDTO?> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return user is not null ? new AppUserDTO
        { 
            Id = user.Id, 
            UserName = user.UserName,
            Email = user.Email 
        } : null;
    }
    
    public async Task<AppUserDTO?> GetUserByUserNameAsync(string userName)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        return user is not null ? new AppUserDTO
        { 
            Id = user.Id, 
            UserName = user.UserName,
            Email = user.Email 
        } : null;
    }

    public async Task<AppUserDTO?> UpdateUserAsync(string id, UpdateUserDTO request)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return null;
        }
        request.UpdateUser(user);

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return null;
        }
        return new AppUserDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }


    public async Task DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is not null)
        {
            await _userManager.DeleteAsync(user);
        }
    }
}
