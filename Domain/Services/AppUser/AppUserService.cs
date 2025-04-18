using DAL;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class AppUserService
{
    private readonly ClothesMarketplaceDbContext _context;

    public AppUserService(ClothesMarketplaceDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<AppUserDTO>> GetAllUsersAsync(int page, int pageSize)
    {
        var query = _context.Users.Select(u => new AppUserDTO
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
        var userById = await _context.AppUsers.FindAsync(id);
        if (userById == null)
            throw new CustomException(CustomExceptionType.NotFound, $"No user found with ID {id}");

        var userDTO = AppUserDTO.FromAppUser(userById);

        return userDTO;
    }
    
    public async Task<AppUserDTO?> GetUserByNameAsync(string userName)
    {
        var userByName = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        if (userByName == null)
            throw new CustomException(CustomExceptionType.NotFound, $"No user found with username {userName}");
        
        var userDTO = AppUserDTO.FromAppUser(userByName);

        return userDTO;
    }

    public async Task<AppUserDTO> CreateCategoryAsync(CreateAppUserDTO request)
    {
        var existingAppUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.UserName == request.UserName);

        if (existingAppUser != null)
            throw new CustomException(CustomExceptionType.IsAlreadyExists, $"User '{request.UserName}' already exists.");
        
        var user = CreateAppUserDTO.ToAppUser(request);
        
        _context.AppUsers.Add(user);
        await _context.SaveChangesAsync();
        
        var newAppUser = await _context.AppUsers.FindAsync(user.Id);
        var userDTO  = AppUserDTO.FromAppUser(newAppUser);

        return userDTO;
    }

    public async Task<AppUserDTO?> UpdateUserAsync(string id, UpdateAppUserDTO request)
    {
        var user = await _context.AppUsers.FindAsync(id);

        if (user == null)
        {
           throw new CustomException(CustomExceptionType.NotFound, $"No user found with ID {id}");
        }
        request.UpdateAppUser(user);

        _context.AppUsers.Update(user);
        await _context.SaveChangesAsync();

        var userDTO = AppUserDTO.FromAppUser(user);

        return userDTO;
    }
     
    public async Task DeleteUserAsync(string id)
    {
        var user = await _context.AppUsers.FindAsync(id);
        if (user == null)
        {
            throw new CustomException(CustomExceptionType.NotFound, $"No user found with ID {id}");
        }
        
        _context.AppUsers.Remove(user);
        await _context.SaveChangesAsync();
    }
}
