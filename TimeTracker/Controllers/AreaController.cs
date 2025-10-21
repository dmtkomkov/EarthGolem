using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Dtos.Area;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;

namespace TimeTracker.Controllers;

[ApiController]
[Route("api/area")]
[Authorize]
public class AreaController(IAreaRepository areaRepo) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var areas = await areaRepo.GetAllAsync();

        var areaDtos = areas.Select(s => s.ToDto());

        return Ok(areaDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id) {
        var areaModel = await areaRepo.GetByIdAsync(id);

        if (areaModel == null) {
            return NotFound();
        }

        return Ok(areaModel.ToDto());
    }
    
    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName([FromRoute] string name) {
        var areaModel = await areaRepo.GetByNameAsync(name);

        if (areaModel == null) {
            return NotFound();
        }

        return Ok(areaModel.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAreaDto areaDto) {
        var areaModel = await areaRepo.CreateAsync(areaDto);

        return Created(string.Empty, areaModel.ToFlatDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateAreaDto areaDto) {
        var areaModel = await areaRepo.UpdateAsync(id, areaDto);

        if (areaModel == null) {
            return NotFound();
        }

        return Ok(areaModel.ToFlatDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id) {
        var areaModel = await areaRepo.DeleteAsync(id);

        if (areaModel == null) {
            return NotFound();
        }

        return NoContent();
    }
}