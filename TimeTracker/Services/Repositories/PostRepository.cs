using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Dtos.Post;
using TimeTracker.Enums;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;
using TimeTracker.Models;

namespace TimeTracker.Services.Repositories;

public class PostRepository(ApplicationDbContext context) : IPostRepository {
    public async Task<PagedList<Post>> GetAllAsync(string postFilter, int pageNumber, int pageSize) {
        IQueryable<Post> query = context.Posts
            .AsNoTracking()
            .Include(p => p.User);

        query = postFilter switch {
            PostParam.Active => query.Where(p => !p.IsDeleted),
            PostParam.Deleted => query.Where(p => p.IsDeleted),
            _ => query
        };

        query = query
            .OrderByDescending(p => p.CreatedOn)
            .ThenByDescending(p => p.Id);

        return await PagedList<Post>.CreateAsync(query, pageNumber, pageSize);
    }

    public async Task<Post?> GetByIdAsync(int id) {
        return await context.Posts
            .AsNoTracking()
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Post?> CreateAsync(CreatePostDto postDto, string userId) {
        var postModel = postDto.ToModel(userId);
        await context.Posts.AddAsync(postModel);
        await context.SaveChangesAsync();

        return await GetByIdAsync(postModel.Id);
    }

    public async Task<Post?> UpdateAsync(int id, UpdatePostDto postDto) {
        var postModel = await context.Posts.FindAsync(id);

        if (postModel == null) {
            return null;
        }

        postModel.UpdateModelFromDto(postDto);
        await context.SaveChangesAsync();

        return await GetByIdAsync(postModel.Id);
    }

    public async Task<Post?> ToggleAsync(int id) {
        var postModel = await context.Posts
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (postModel == null) {
            return null;
        }

        postModel.IsDeleted = !postModel.IsDeleted;
        await context.SaveChangesAsync();
        return postModel;
    }

    public async Task<Post?> DeleteAsync(int id) {
        var postModel = await context.Posts.FindAsync(id);

        if (postModel == null) {
            return null;
        }

        context.Posts.Remove(postModel);
        await context.SaveChangesAsync();
        return postModel;
    }
}
