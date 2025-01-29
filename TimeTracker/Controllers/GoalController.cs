using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Dtos.Goal;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;

namespace TimeTracker.Controllers;

[ApiController]
[Route("api/goal")]
[Authorize]
public class GoalController(IGoalRepository goalRepo) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var goals = await goalRepo.GetAllAsync();

        var goalDtos = goals.Select(c => c.ToDto());

        return Ok(goalDtos);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id) {
        var goalModel = await goalRepo.GetByIdAsync(id);

        if (goalModel == null) {
            return NotFound();
        }

        return Ok(goalModel.ToDto());
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGoalDto goalDto) {
        var goalModel = await goalRepo.CreateAsync(goalDto);

        if (goalModel == null) {
            return NotFound();
        }
        
        return Created(string.Empty, goalModel.ToDto());
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateGoalDto goalDto) {
        var goalModel = await goalRepo.UpdateAsync(id, goalDto);

        if (goalModel == null) {
            return NotFound();
        }
        
        return Ok(goalModel.ToDto());
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id) {
        var goalModel = await goalRepo.DeleteAsync(id);
        
        if (goalModel == null) {
            return NotFound();
        }

        return NoContent();
    }
}