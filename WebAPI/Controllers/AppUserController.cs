using DAL;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AppUserController : ControllerBase
{
    private readonly AppUserService _userService;

    public AppUserController(AppUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<AppUser>>> GetUsers()
    {
        return Ok(await _userService.GetAllUsersAsync());
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,User")]
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


    [HttpPost]
    [AllowAnonymous] 
    public async Task<IActionResult> CreateUser([FromBody] AppUser user, [FromQuery] string password)
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            return BadRequest("Ви вже авторизовані і не можете створювати нового користувача.");
        }

        if (await _userService.CreateUserAsync(user, password))
        {
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        return BadRequest("Не вдалося створити користувача");
    }


    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] AppUser user)
    {
        if (id != user.Id && !User.IsInRole("Admin"))
            return BadRequest("ID не співпадають");

        if (await _userService.UpdateUserAsync(user))
            return NoContent();

        return BadRequest("Не вдалося оновити користувача");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        if (id != User.Identity.Name && !User.IsInRole("Admin"))
            return BadRequest("Ви не можете видаляти цього користувача");

        if (await _userService.DeleteUserAsync(id))
            return NoContent();

        return NotFound();
    }
}
