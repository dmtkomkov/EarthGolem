using TimeTracker.Dtos.Category;
using TimeTracker.Models;

namespace TimeTracker.Interfaces;

public interface ICategoryRepository {
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category?> CreateAsync(CreateCategoryDto categoryDto);
    Task<Category?> UpdateAsync(int id, UpdateCategoryDto categoryDto);
    Task<Category?> DeleteAsync(int id);
    Task<bool> CategoryExists(int id);
}