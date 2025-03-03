using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Mappers;

namespace TimeTracker.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController(UserManager<IdentityUser> userManager) : ControllerBase {
    [HttpGet]
    public async Task<ActionResult> GetAllUsersAsync()
    {
        var users = await userManager.Users.ToListAsync();

        var userDtos = users.Select(u => u.ToDto());
        return Ok(userDtos);
    }
}