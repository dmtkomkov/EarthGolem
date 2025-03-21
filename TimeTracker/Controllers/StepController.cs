﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Dtos.Step;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;

namespace TimeTracker.Controllers;

[ApiController]
[Route("api/step")]
[Authorize]
public class StepController(IStepRepository stepRepo, UserManager<IdentityUser> userManager) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] DateOnly? date) {
        var steps = await stepRepo.GetAllAsync(date);

        var stepDtos = steps.Select(s => s.ToDto());

        return Ok(stepDtos);
    }
    
    [HttpGet("group")]
    public async Task<IActionResult> GetAllGroupedByDate() {
        var stepGroups = await stepRepo.GetAllGroupedByDateAsync();

        var stepGroupDtos = stepGroups.Select(sg => sg.ToGroupDto());

        return Ok(stepGroupDtos);
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
        if (string.IsNullOrWhiteSpace(stepDto.UserId)) {
            // Set userId from current user
            var user = await userManager.GetUserAsync(User);

            if (user == null) {
                return BadRequest("Cannot create step without user");
            }
            
            stepDto.UserId = user.Id;
        }
        
        var stepModel = await stepRepo.CreateAsync(stepDto);

        if (stepModel == null) {
            return BadRequest("Step is not created");
        }
        
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