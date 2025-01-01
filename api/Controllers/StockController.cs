using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/stock")]
public class StockController : ControllerBase {
    private readonly ApplicationDBContext _context;
    
    public StockController(ApplicationDBContext context) {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll() {
        var stocks = _context.Stocks.ToList().Select(s => s.ToStockDto());

        return Ok(stocks);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById([FromRoute] int id) {
        var stock = _context.Stocks.Find(id);

        return stock == null ? NotFound() : Ok(stock);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateStockDto stockDto) {
        var stockModel = stockDto.ToStock();
        _context.Stocks.Add(stockModel);
        _context.SaveChanges();
        
        return Created(string.Empty, stockModel.ToStockDto());
    }

    [HttpPut("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockDto updateStockDto) {
        var stockModel = _context.Stocks.FirstOrDefault(s => s.Id == id);

        if (stockModel == null) {
            return NotFound();
        }

        stockModel.Symbol = updateStockDto.Symbol;
        stockModel.CompanyName = updateStockDto.CompanyName;
        stockModel.Purchase = updateStockDto.Purchase;
        stockModel.LastDiv = updateStockDto.LastDiv;
        stockModel.Industry = updateStockDto.Industry;
        stockModel.MarketCap = updateStockDto.MarketCap;

        _context.SaveChanges();

        return Ok(stockModel.ToStockDto());
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete([FromRoute] int id) {
        var stockModel = _context.Stocks.FirstOrDefault(s => s.Id == id);
        
        if (stockModel == null) {
            return NotFound();
        }

        _context.Stocks.Remove(stockModel);

        _context.SaveChanges();

        return NoContent();
    }
}