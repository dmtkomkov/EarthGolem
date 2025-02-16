using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Dtos.Category;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;
using TimeTracker.Models;

namespace TimeTracker.Services.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository {
    public async Task<List<Category>> GetAllAsync(string? areaFilter) {
        IQueryable<Category> query = context.Categories
            .Include(c => c.Area);

        if (!string.IsNullOrWhiteSpace(areaFilter)) {
            query = query.Where(c => c.Area!.Name == areaFilter);
        }

        return await query
            .OrderByDescending(c => c.Id)
            .ToListAsync();
    }
    
    public async Task<List<CategoryGroup>> GetAllGroupedByAreaAsync() {
        var areas = await context.Areas
            .AsNoTracking()
            .OrderByDescending(a => a.Id)
            .Select(a => a.Name)
            .ToListAsync();
        
        var groupedCategories = await context.Categories
            .AsNoTracking()
            .Include(c => c.Area)
            .Where(c => areas.Contains(c.Area!.Name))
            .GroupBy(c => c.Area)
            .Select(g => new CategoryGroup() {
                Area = g.Key,
                Categories = g.OrderByDescending(c => c.Id).ToList()
            })
            .OrderByDescending(g => g.Area!.Id)
            .ToListAsync();

        return groupedCategories;
    }

    public async Task<Category?> GetByIdAsync(int id) {
        return await context.Categories
            .Include(c => c.Area)
            .FirstOrDefaultAsync(c => c.Id == id);
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