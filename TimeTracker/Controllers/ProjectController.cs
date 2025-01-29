using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Dtos.Project;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;

namespace TimeTracker.Controllers;

[ApiController]
[Route("api/project")]
[Authorize]
public class ProjectController(IProjectRepository projectRepo) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var projects = await projectRepo.GetAllAsync();

        var projectDtos = projects.Select(p => p.ToDto());

        return Ok(projectDtos);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id) {
        var projectModels = await projectRepo.GetByIdAsync(id);

        if (projectModels == null) {
            return NotFound();
        }

        return Ok(projectModels.ToDto());
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto projectDto) {
        var projectModel = await projectRepo.CreateAsync(projectDto);
        
        return Created(string.Empty, projectModel.ToDto());
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProjectDto projectDto) {
        var projectModel = await projectRepo.UpdateAsync(id, projectDto);

        if (projectModel == null) {
            return NotFound();
        }
        
        return Ok(projectModel.ToDto());
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id) {
        var projectModel = await projectRepo.DeleteAsync(id);
        
        if (projectModel == null) {
            return NotFound();
        }

        return NoContent();
    }
}