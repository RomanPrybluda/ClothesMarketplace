using DAL;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Produces("application/json")]
[Route("AppUser")]
public class AppUserController : ControllerBase
{
    private readonly AppUserService _userService;

    public AppUserController(AppUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
//    [Authorize]
    public async Task<ActionResult<AppUser>> GetUser([Required] string id)
    {
/*        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized("User ID not found, please authorized.");
*/
    
        var userDTO = await _userService.GetUserByIdAsync(id);
        if (userDTO == null)
                return NotFound($"No user found with ID {id}");

        return Ok(userDTO);
    }
    
    [HttpGet("{userName}")]
 //   [Authorize]
    public async Task<ActionResult<AppUser>> GetUserByName([Required] string userName)
    {
 /*       var currentUserName = User.Identity?.Name;

        if (currentUserName != userName)
        {
            return Forbid();        
        }
*/
        var userDTO = await _userService.GetUserByNameAsync(userName);
        if (userDTO == null)
            return NotFound($"No user found with username {userName}");

        return Ok(userDTO);
    }

    
    
    [HttpPut("{id}")]
 //   [Authorize]
    public async Task<ActionResult> UpdateUserAsync(string id, [FromBody][Required] UpdateAppUserDTO request)
    {
/*        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if(userId != id)
        {
            return Forbid();
        }
 */           
        var updatedUserDTO = await _userService.UpdateUserAsync(id, request);

        if(updatedUserDTO == null)
        {
            return NotFound($"No user found with ID {id}");
        }
            
        return Ok(updatedUserDTO);
    }
}
