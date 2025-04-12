using DAL;
using Domain;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Produces("application/json")]
[Route("Admin")]
//[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly AppUserService _userService;

    public AdminController(AppUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("users")]
    public async Task<ActionResult<PagedResult<AppUserDTO>>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var users = await _userService.GetAllUsersAsync(page, pageSize);
        return Ok(users);
    }

    [HttpDelete("user/{id}")]
    public async Task<IActionResult> DeleteUser([Required] string id)
    {
        var user = await _userService.GetUserByIdAsync(id); 
        if (user is null)
            return NotFound(new { Message = $"No user found with ID {id}" });

        await _userService.DeleteUserAsync(id);
        return NoContent();
    }


    [HttpGet("user/{id}")]
    public async Task<ActionResult<AppUser>> GetUser([Required] string id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }
}
