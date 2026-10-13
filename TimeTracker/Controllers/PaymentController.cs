using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Dtos.Payment;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;

namespace TimeTracker.Controllers;

[ApiController]
[Route("api/payment")]
[Authorize]
public class PaymentController(
    IPaymentRepository paymentRepo,
    IPaymentCategoryRepository categoryRepo
) : ControllerBase {
    private const string CategoryNotFound = "Payment category not found";

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] DateOnly? date) {
        var payments = await paymentRepo.GetAllAsync(date);

        var paymentDtos = payments.Select(p => p.ToDto());

        return Ok(paymentDtos);
    }

    [HttpGet("group")]
    public async Task<IActionResult> GetAllGroupedByDate(
        [FromQuery] [Range(1, int.MaxValue)] int pageNumber = 1,
        [FromQuery] [Range(1, 100)] int pageSize = 20
    ) {
        var pagedPaymentGroup = await paymentRepo.GetAllGroupedByDateAsync(pageNumber, pageSize);

        return Ok(pagedPaymentGroup.ToDto());
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetMonthlyReport(
        [FromQuery] [Range(1, int.MaxValue)] int pageNumber = 1,
        [FromQuery] [Range(1, 100)] int pageSize = 20
    ) {
        var pagedMonthReport = await paymentRepo.GetMonthlyReportAsync(pageNumber, pageSize);

        return Ok(pagedMonthReport.ToDto());
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] DateOnly? date) {
        var paymentSummary = await paymentRepo.GetSummaryAsync(date ?? DateOnly.FromDateTime(DateTime.Today));

        return Ok(paymentSummary.ToDto());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id) {
        var paymentModel = await paymentRepo.GetByIdAsync(id);

        if (paymentModel == null) {
            return NotFound();
        }

        return Ok(paymentModel.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentDto paymentDto) {
        if (!await categoryRepo.PaymentCategoryExists(paymentDto.CategoryId)) {
            return BadRequest(CategoryNotFound);
        }

        var paymentModel = await paymentRepo.CreateAsync(paymentDto);

        if (paymentModel == null) {
            return BadRequest("Payment is not created");
        }

        return Created(string.Empty, paymentModel.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePaymentDto paymentDto) {
        if (!await categoryRepo.PaymentCategoryExists(paymentDto.CategoryId)) {
            return BadRequest(CategoryNotFound);
        }

        var paymentModel = await paymentRepo.UpdateAsync(id, paymentDto);

        if (paymentModel == null) {
            return NotFound();
        }

        return Ok(paymentModel.ToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id) {
        var paymentModel = await paymentRepo.DeleteAsync(id);

        if (paymentModel == null) {
            return NotFound();
        }

        return NoContent();
    }
}
