using TimeTracker.Dtos.Post;
using TimeTracker.Models;

namespace TimeTracker.Interfaces;

public interface IPostRepository {
    Task<PagedList<Post>> GetAllAsync(string postFilter, int pageNumber, int pageSize);
    Task<Post?> GetByIdAsync(int id);
    Task<Post?> CreateAsync(CreatePostDto postDto, string userId);
    Task<Post?> UpdateAsync(int id, UpdatePostDto postDto);
    Task<Post?> ToggleAsync(int id);
    Task<Post?> DeleteAsync(int id);
}
