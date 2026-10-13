using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Dtos.PaymentCategory;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;

namespace TimeTracker.Controllers;

[ApiController]
[Route("api/payment-category")]
[Authorize]
public class PaymentCategoryController(IPaymentCategoryRepository categoryRepo) : ControllerBase {
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
    public async Task<IActionResult> Create([FromBody] CreatePaymentCategoryDto categoryDto) {
        var conflict = await FindConflictAsync(categoryDto.Name, categoryDto.Label, null);

        if (conflict != null) {
            return Conflict(conflict);
        }

        var categoryModel = await categoryRepo.CreateAsync(categoryDto);

        return Created(string.Empty, categoryModel.ToFlatDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePaymentCategoryDto categoryDto) {
        if (!await categoryRepo.PaymentCategoryExists(id)) {
            return NotFound();
        }

        var conflict = await FindConflictAsync(categoryDto.Name, categoryDto.Label, id);

        if (conflict != null) {
            return Conflict(conflict);
        }

        var categoryModel = await categoryRepo.UpdateAsync(id, categoryDto);

        if (categoryModel == null) {
            return NotFound();
        }

        return Ok(categoryModel.ToFlatDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id) {
        if (!await categoryRepo.PaymentCategoryExists(id)) {
            return NotFound();
        }

        if (await categoryRepo.HasPaymentsAsync(id)) {
            return Conflict("Payment category has payments and cannot be deleted");
        }

        var categoryModel = await categoryRepo.DeleteAsync(id);

        if (categoryModel == null) {
            return NotFound();
        }

        return NoContent();
    }

    private async Task<string?> FindConflictAsync(string name, string? label, int? excludeId) {
        if (await categoryRepo.NameExistsAsync(name, excludeId)) {
            return "Payment category name already exists";
        }

        if (await categoryRepo.LabelExistsAsync(label, excludeId)) {
            return "Payment category label already exists";
        }

        return null;
    }
}
