using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/stock")]
public class StockController : ControllerBase {
    private readonly IStockRepository _stockRepo;

    public StockController(IStockRepository stockRepo) {
        _stockRepo = stockRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query) {
        var stockModels = await _stockRepo.GetAllAsync(query);
        var stockDtos = stockModels.Select(s => s.ToDto());

        return Ok(stockDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id) {
        var stockModel = await _stockRepo.GetByIdAsync(id);

        return stockModel == null ? NotFound() : Ok(stockModel.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockDto stockDto) {
        var stockModel = stockDto.ToModel();

        stockModel = await _stockRepo.CreateAsync(stockModel);
        
        return Created(string.Empty, stockModel.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto stockDto) {
        var stockModel = await _stockRepo.UpdateAsync(id, stockDto);

        if (stockModel == null) {
            return NotFound();
        }
        
        return Ok(stockModel.ToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id) {
        var stockModel = await _stockRepo.DeleteAsync(id);
        
        if (stockModel == null) {
            return NotFound();
        }

        return NoContent();
    }
}