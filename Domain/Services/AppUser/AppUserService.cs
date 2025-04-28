using AutoMapper;
using DAL;
using DAL.Repository;
using Domain;
using Domain.Ñommon.Wrappers;
using Microsoft.EntityFrameworkCore;

public class AppUserService
{
    private readonly ClothesMarketplaceDbContext _context;
    private readonly UserRepository _userRepository;
    private readonly IMapper _mapper;

    public AppUserService(ClothesMarketplaceDbContext context, UserRepository userRepository, IMapper mapper)
    {
        _context = context;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    //public async Task<PagedResult<AppUserDTO>> GetAllUsersAsync(int page, int pageSize)
    //{
    //    List<AppUserDTO> userDTOs = new();

    //    var users = await _userRepository.GetUsersAsync(page, pageSize);
    //    var totalUsers = await _userRepository.GetUserCount();

    //    if (users == null || !users.Any())
    //        throw new CustomException(CustomExceptionType.NotFound, "No users found.");

    //    foreach (var user in users)
    //    {
    //        var userDTO = _mapper.Map<AppUserDTO>(user);
    //        userDTOs.Add(userDTO);
    //    }

    //    return new PagedResult<AppUserDTO> { Items = userDTOs, TotalCount = totalUsers };
    //}

    //public async Task<AppUserDTO?> GetUserByIdAsync(Guid id)
    //{
    //    var userById = await _userRepository.FindByIdAsync(id);
    //    if (userById == null)
    //        throw new CustomException(CustomExceptionType.NotFound, $"No user found with ID {id}");

    //    var userDTO = AppUserDTO.FromAppUser(userById);

    //    return userDTO;
    //}

    //public async Task<AppUserDTO?> GetUserByNameAsync(string userName)
    //{
    //    var userByName = await _userRepository.FindByNameAsync(userName);
    //    if (userByName == null)
    //        throw new CustomException(CustomExceptionType.NotFound, $"No user found with username {userName}");

    //    var userDTO = AppUserDTO.FromAppUser(userByName);

    //    return userDTO;
    //}

    //TODO: SHOULD ADD TO ROLE WHEN DEFAULT ROLE IS USER
    // METHOD MUST GET ROLE NAME, PASSWORD AND USER

    public async Task<AppUserDTO> CreateUserAsync(CreateAppUserDTO request)
    {
        var existingAppUser = await _userRepository.FindByEmailAsync(request.Email);

        if (existingAppUser != null)
            throw new CustomException(CustomExceptionType.IsAlreadyExists, $"User '{request.UserName}' already exists.");

        var user = _mapper.Map<AppUser>(request);

        _context.AppUsers.Add(user);
        await _context.SaveChangesAsync();

        var newAppUser = await _context.AppUsers.FindAsync(user.Id);
        var response = _mapper.Map<AppUserDTO>(newAppUser);

        return response;
    }

    //public async Task<AppUserDTO> UpdateUserAsync(Guid id, UpdateAppUserDTO request)
    //{
    //    var existingUser = await _userRepository.FindByIdAsync(id);

    //    if (existingUser == null)
    //        throw new CustomException(CustomExceptionType.NotFound, $"No user found with ID {id}");

    //    var user = _mapper.Map<AppUser>(request);
    //    var result = await _userRepository.UpdateUserAsync(user);

    //    if (!result.Succeeded)
    //        throw new CustomException(CustomExceptionType.InternalError, $"Failed to update user with ID {id}");

    //    var updatedUser = await _userRepository.FindByIdAsync(id);
    //    var response = _mapper.Map<AppUserDTO>(updatedUser);

    //    return response;
    //}

    //public async Task DeleteUserAsync(Guid id)
    //{
    //    var user = await _userRepository.DeleteUserAsync(id);
        
    //    if (!user.Succeeded)
    //        throw new CustomException(CustomExceptionType.NotFound, $"No user found with ID {id}");
    //}
}
