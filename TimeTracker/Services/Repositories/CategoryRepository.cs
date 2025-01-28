using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Dtos.Category;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;
using TimeTracker.Models;

namespace TimeTracker.Services.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository {
    public async Task<List<Category>> GetAllAsync() {
        return await context.Categories.ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id) {
        return await context.Categories.FindAsync(id);
    }

    public async Task<Category?> CreateAsync(CreateCategoryDto categoryDto) {
        var areaModel = await context.Areas.FindAsync(categoryDto.AreaId);
        
        if (areaModel == null) {
            return null;
        }
        
        var categoryModel = categoryDto.ToModel();
        await context.Categories.AddAsync(categoryModel);
        await context.SaveChangesAsync();
        return categoryModel;
    }

    public async Task<Category?> UpdateAsync(int id, UpdateCategoryDto categoryDto) {
        var categoryModel = await context.Categories.FindAsync(id);
        var areaModel = await context.Areas.FindAsync(categoryDto.AreaId);
        
        if (categoryModel == null || areaModel == null) {
            return null;
        }

        categoryModel.UpdateModelFromDto(categoryDto);
        await context.SaveChangesAsync();

        return categoryModel;
    }

    public async Task<Category?> DeleteAsync(int id) {
        var categoryModel = await context.Categories.FindAsync(id);

        if (categoryModel == null) {
            return null;
        }

        context.Categories.Remove(categoryModel);
        await context.SaveChangesAsync();

        return categoryModel;
    }

    public async Task<bool> CategoryExists(int id) {
        return await context.Categories.AnyAsync(a => a.Id == id);
    }
}