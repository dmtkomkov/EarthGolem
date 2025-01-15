using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Dtos;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;

namespace TimeTracker.Controllers;

[ApiController]
[Route("api/step")]
[Authorize]
public class StepController(IStepRepository stepRepo) : ControllerBase
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
        var stepModel = await stepRepo.CreateAsync(stepDto);
        
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
        var stockModel = await stepRepo.DeleteAsync(id);
        
        if (stockModel == null) {
            return NotFound();
        }

        return NoContent();
    }
}