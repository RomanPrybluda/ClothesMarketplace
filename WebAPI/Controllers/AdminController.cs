using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly AppUserService _userService;

    public AdminController(AppUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("users")]
    public async Task<ActionResult<List<AppUser>>> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] AppUser user, [FromQuery] string password)
    {
        if (await _userService.CreateUserAsync(user, password))
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);

        return BadRequest("Не вдалося створити користувача");
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] AppUser user)
    {
        if (id != user.Id)
            return BadRequest("ID не співпадають");

        if (await _userService.UpdateUserAsync(user))
            return NoContent();

        return BadRequest("Не вдалося оновити користувача");
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        if (await _userService.DeleteUserAsync(id))
            return NoContent();

        return NotFound();
    }

    [HttpGet("user/{id}")]
    public async Task<ActionResult<AppUser>> GetUser(string id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }
}
