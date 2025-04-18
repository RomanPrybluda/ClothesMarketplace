using DAL;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Produces("application/json")]
[Route("admin/users")]
[Authorize]

public class AdminController : ControllerBase
{
    private readonly AppUserService _userService;

    public AdminController(AppUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<AppUserDTO>>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var users = await _userService.GetAllUsersAsync(page, pageSize);
        return Ok(users);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteUser([Required] Guid id)
    {
        await _userService.GetUserByIdAsync(id);
        return NoContent();
    }


    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<AppUser>> GetUser([Required] Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return Ok(user);
    }
}
