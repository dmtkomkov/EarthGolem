using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Dtos.Area;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;
using TimeTracker.Models;

namespace TimeTracker.Services.Repositories;

public class AreaRepository(ApplicationDbContext context) : IAreaRepository {
    public async Task<List<Area>> GetAllAsync() {
        return await context.Areas.Include(a => a.Categories).ToListAsync();
    }

    public async Task<Area?> GetByIdAsync(int id) {
        return await context.Areas.Include(a => a.Categories).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Area> CreateAsync(CreateAreaDto areaDto) {
        var areaModel = areaDto.ToModel();
        await context.Areas.AddAsync(areaModel);
        await context.SaveChangesAsync();
        return areaModel;
    }

    public async Task<Area?> UpdateAsync(int id, UpdateAreaDto areaDto) {
        var areaModel = await context.Areas.FindAsync(id);

        if (areaModel == null)
        {
            return null;
        }

        areaModel.UpdateModelFromDto(areaDto);
        await context.SaveChangesAsync();

        return areaModel;
    }

    public async Task<Area?> DeleteAsync(int id) {
        var areaModel = await context.Areas.FindAsync(id);

        if (areaModel == null) {
            return null;
        }

        context.Areas.Remove(areaModel);
        await context.SaveChangesAsync();

        return areaModel;
    }

    public async Task<bool> AreaExists(int id) {
        return await context.Areas.AnyAsync(a => a.Id == id);
    }
}