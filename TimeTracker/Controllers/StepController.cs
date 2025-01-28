using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Dtos;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;

namespace TimeTracker.Controllers;

[ApiController]
[Route("api/step")]
[Authorize]
public class StepController(IStepRepository stepRepo, UserManager<IdentityUser> userManager) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var steps = await stepRepo.GetAllAsync();

        var stepDtos = steps.Select(s => s.ToDto());

        return Ok(stepDtos);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id) {
        var stepModel = await stepRepo.GetByIdAsync(id);

        if (stepModel == null) {
            return NotFound();
        }

        return Ok(stepModel.ToDto());
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStepDto stepDto) {
        var user = await userManager.GetUserAsync(User);

        if (user == null) {
            return BadRequest("Cannot create step without user");
        }

        var stepModel = await stepRepo.CreateAsync(stepDto, user.Id);
        
        return Created(string.Empty, stepModel.ToDto());
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStepDto stepDto) {
        var stepModel = await stepRepo.UpdateAsync(id, stepDto);

        if (stepModel == null) {
            return NotFound();
        }
        
        return Ok(stepModel.ToDto());
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id) {
        var stepModel = await stepRepo.DeleteAsync(id);
        
        if (stepModel == null) {
            return NotFound();
        }

        return NoContent();
    }
}