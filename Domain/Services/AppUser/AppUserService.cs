using AutoMapper;
using DAL;
using DAL.Repository;
using Domain;
using Domain.Сommon.Wrappers;
using Microsoft.AspNetCore.Identity;

public class AppUserService(UserRepository _userRepository, IMapper _mapper)
{
    public async Task<PagedResult<AppUserDTO>> GetAllUsersAsync(int page, int pageSize)
    {
        var users = await _userRepository.GetUsersAsync(page, pageSize);
        var totalUsers = await _userRepository.GetUserCount();

        if (users?.Any() != true)
            throw new CustomException(CustomExceptionType.NotFound, "No users found.");

        var userDTOs = _mapper.Map<List<AppUserDTO>>(users);

        return new PagedResult<AppUserDTO> { Items = userDTOs, TotalCount = totalUsers };
    }

    public async Task<AppUserDTO> GetUserByIdAsync(string id)
    {
        var userById = await _userRepository.FindByIdAsync(id);
        if (userById == null)
            throw new CustomException(CustomExceptionType.NotFound, $"No user found with ID {id}");

        var response = _mapper.Map<AppUserDTO>(userById);

        return response;
    }

    public async Task<AppUserDTO> GetUserByNameAsync(string userName)
    {
        var userByName = await _userRepository.FindByNameAsync(userName);
        if (userByName == null)
            throw new CustomException(CustomExceptionType.NotFound, $"No user found with username {userName}");

        var response = _mapper.Map<AppUserDTO>(userByName);

        return response;
    }

    public async Task<Result<AppUserDTO>> CreateUserAsync(AppUser appUser, string password, IdentityRole userRole)
    {
        var existingAppUser = await _userRepository.FindByEmailAsync(appUser.Email);

        if (existingAppUser != null)
            throw new CustomException(CustomExceptionType.IsAlreadyExists, $"User '{appUser.UserName}' already exists.");

        var userCreateResult = await _userRepository.CreateUserAsync(appUser, password);

        if (!userCreateResult.Succeeded)
            return Result<AppUserDTO>.Failure(userCreateResult.Errors);

        var userRoleResult = await _userRepository.AddToRoleAsync(appUser, userRole.Name);

        if (!userRoleResult.Succeeded)
            throw new CustomException(CustomExceptionType.FaildToAddRoleToUser, $"Failed to add user '{appUser.UserName}' to role '{userRole.Name}'.");

        var newAppUser = await _userRepository.FindByEmailAsync(appUser.Email);
        var response = _mapper.Map<AppUserDTO>(newAppUser);

        return Result<AppUserDTO>.Success(response);
    }

    public async Task<AppUserDTO> UpdateUserAsync(string id, AppUser userForUpdate)
    {
        var existingUser = await _userRepository.FindByIdAsync(id);

        if (existingUser == null)
            throw new CustomException(CustomExceptionType.NotFound, $"No user found with ID {id}");

        var user = _mapper.Map<AppUser>(userForUpdate);
        var result = await _userRepository.UpdateUserAsync(user);

        if (!result.Succeeded)
            throw new CustomException(CustomExceptionType.InternalError, $"Failed to update user with ID {id}");

        var updatedUser = await _userRepository.FindByIdAsync(id);
        var response = _mapper.Map<AppUserDTO>(updatedUser);

        return response;
    }

    public async Task DeleteUserAsync(string id)
    {
        var user = await _userRepository.DeleteUserAsync(id);

        if (!user.Succeeded)
            throw new CustomException(CustomExceptionType.NotFound, $"No user found with ID {id}");
    }
}
