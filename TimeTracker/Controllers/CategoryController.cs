using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Dtos.Category;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;

namespace TimeTracker.Controllers;

[ApiController]
[Route("api/category")]
[Authorize]
public class CategoryController(ICategoryRepository categoryRepo) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var categories = await categoryRepo.GetAllAsync();

        var categoryDtos = categories.Select(c => c.ToDto());

        return Ok(categoryDtos);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id) {
        var categoryModel = await categoryRepo.GetByIdAsync(id);

        if (categoryModel == null) {
            return NotFound();
        }

        return Ok(categoryModel.ToDto());
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto categoryDto) {
        var categoryModel = await categoryRepo.CreateAsync(categoryDto);

        if (categoryModel == null) {
            return NotFound();
        }
        
        return Created(string.Empty, categoryModel.ToFlatDto());
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryDto categoryDto) {
        var categoryModel = await categoryRepo.UpdateAsync(id, categoryDto);

        if (categoryModel == null) {
            return NotFound();
        }
        
        return Ok(categoryModel.ToFlatDto());
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id) {
        var categoryModel = await categoryRepo.DeleteAsync(id);
        
        if (categoryModel == null) {
            return NotFound();
        }

        return NoContent();
    }
}