using DAL;
using DAL.Models;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Extentions;

[ApiController]
[Produces("application/json")]
[Route("user/admin")]
[Authorize(Roles = "Admin")]

public class AdminController : ControllerBase
{
    private readonly AppUserService _userService;
    private readonly AuthService _authService;

    public AdminController(AppUserService userService, AuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> AddAdmin([FromBody][Required] RegistrationDTO request)
    {
        var userRole = RoleRegistry.Admin;
        var result = await _authService.RegisterAsync(request, userRole);
        return result.ToResponse();
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
        await _userService.GetUserByIdAsync(id.ToString());
        return NoContent();
    }


    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<AppUser>> GetUser([Required] Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id.ToString());
        return Ok(user);
    }
}
