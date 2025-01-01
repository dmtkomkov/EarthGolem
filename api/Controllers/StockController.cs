using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("api/stock")]
public class StockController : ControllerBase {
    private readonly ApplicationDBContext _context;
    
    public StockController(ApplicationDBContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var stockModels = await _context.Stocks.ToListAsync();
        var stockDtos = stockModels.Select(s => s.ToStockDto());

        return Ok(stockDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id) {
        var stock = await _context.Stocks.FindAsync(id);

        return stock == null ? NotFound() : Ok(stock);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockDto stockDto) {
        var stockModel = stockDto.ToStock();
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();
        
        return Created(string.Empty, stockModel.ToStockDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto updateStockDto) {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

        if (stockModel == null) {
            return NotFound();
        }

        stockModel.UpdateFromDto(updateStockDto);

        await _context.SaveChangesAsync();

        return Ok(stockModel.ToStockDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id) {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
        
        if (stockModel == null) {
            return NotFound();
        }

        _context.Stocks.Remove(stockModel);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}