using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Produces("application/json")]
[Route("/user")]
[Authorize]

public class AppUserController : ControllerBase
{
    private readonly AppUserService _userService;

    public AppUserController(AppUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult> GetUser([Required] Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id.ToString());
        return Ok(user);
    }

    [HttpGet("{userName}")]
    public async Task<ActionResult> GetUserByName([Required] string userName)
    {
        var user = await _userService.GetUserByNameAsync(userName);
        return Ok(user);
    }

    //[HttpPut("{id:Guid}")]
    //public async Task<ActionResult> UpdateUserAsync([Required] Guid id, [FromBody][Required] UpdateAppUserDTO request)
    //{
    //    var user = await _userService.UpdateUserAsync(id.ToString(), request);
    //    return Ok(user);
    //}
}
