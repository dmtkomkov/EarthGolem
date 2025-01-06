using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/comment")]
public class CommentController : ControllerBase {
    private readonly ICommentRepository _commentRepo;
    private readonly IStockRepository _stockRepo;

    public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo) {
        _commentRepo = commentRepo;
        _stockRepo = stockRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        var comments = await _commentRepo.GetAllAsync();

        var commentDto = comments.Select(c => c.ToDto());

        return Ok(commentDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id) {
        var commentModel = await _commentRepo.GetByIdAsync(id);

        if (commentModel == null) {
            return NotFound();
        }

        return Ok(commentModel.ToDto());
    }

    [HttpPost("{stockId:int}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        if (!await _stockRepo.StockExists(stockId)) {
            return BadRequest($"Stock {stockId} does not exist");
        }

        var commentModel = commentDto.ToModel(stockId);
        await _commentRepo.CreateAsync(commentModel);
        
        return Created(string.Empty, commentModel.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto commentDto) {
        var commentModel = await _commentRepo.UpdateAsync(id, commentDto);

        if (commentModel == null) {
            return NotFound($"Comment {id} not found");
        }

        return Ok(commentModel.ToDto());
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id) {
        var commentModel = await _commentRepo.DeleteAsync(id);
        
        if (commentModel == null) {
            return NotFound();
        }

        return NoContent();
    }
}