using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Dtos.Post;
using TimeTracker.Enums;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;

namespace TimeTracker.Controllers;

[ApiController]
[Route("api/post")]
[Authorize]
public class PostController(IPostRepository postRepo, UserManager<IdentityUser> userManager) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string showPosts = PostParam.Active,
        [FromQuery] [Range(1, int.MaxValue)] int pageNumber = 1,
        [FromQuery] [Range(1, 100)] int pageSize = 20
    ) {
        var pagedPosts = await postRepo.GetAllAsync(showPosts, pageNumber, pageSize);

        return Ok(pagedPosts.ToDto());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id) {
        var postModel = await postRepo.GetByIdAsync(id);

        if (postModel == null) {
            return NotFound();
        }

        return Ok(postModel.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostDto postDto) {
        // The author is always the current user
        var user = await userManager.GetUserAsync(User);

        if (user == null) {
            return BadRequest("Cannot create post without user");
        }

        var postModel = await postRepo.CreateAsync(postDto, user.Id);

        if (postModel == null) {
            return BadRequest("Post is not created");
        }

        return Created(string.Empty, postModel.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePostDto postDto) {
        var postModel = await postRepo.UpdateAsync(id, postDto);

        if (postModel == null) {
            return NotFound();
        }

        return Ok(postModel.ToDto());
    }

    [HttpPut("toggle/{id:int}")]
    public async Task<IActionResult> Toggle([FromRoute] int id) {
        var postModel = await postRepo.ToggleAsync(id);

        if (postModel == null) {
            return NotFound();
        }

        return Ok(postModel.ToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id) {
        var postModel = await postRepo.DeleteAsync(id);

        if (postModel == null) {
            return NotFound();
        }

        return NoContent();
    }
}
