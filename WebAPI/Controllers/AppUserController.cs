using DAL;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("AppUser")]
[ApiController]
public class AppUserController : ControllerBase
{
    private readonly AppUserService _userService;
    private readonly UserManager<AppUser> _userManager;

    public AppUserController(AppUserService userService, UserManager<AppUser> userManager)
    {
        _userService = userService;
        _userManager = userManager;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<AppUser>> GetUser(string id)
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Admin") || User.Identity.Name == id)
            {
                var user = await _userService.GetUserByIdAsync(id);
                return user == null ? NotFound() : Ok(user);
            }
        }
        return Unauthorized();
    }
    
    [HttpGet("{userName}")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<AppUser>> GetUserByName(string userName)
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Admin") || User.Identity.Name == userName)
            {
                var user = await _userService.GetUserByUserNameAsync(userName);
                return user == null ? NotFound() : Ok(user);
            }
        }
        return Unauthorized();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDTO request)
    {
        if (request == null)
        {
            return BadRequest("Invalid request data");
        }

        var updatedUser = await _userService.UpdateUserAsync(id, request);
        
        if (updatedUser == null)
        {
            return NotFound("User not found or update failed");
        }

        return Ok(updatedUser);
    }
}
