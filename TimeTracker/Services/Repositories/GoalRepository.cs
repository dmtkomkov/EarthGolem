using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Dtos.Goal;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;
using TimeTracker.Models;

namespace TimeTracker.Services.Repositories;

public class GoalRepository(ApplicationDbContext context) : IGoalRepository {
    public async Task<List<Goal>> GetAllAsync() {
        return await context.Goals.ToListAsync();
    }

    public async Task<Goal?> GetByIdAsync(int id) {
        return await context.Goals.FindAsync(id);
    }

    public async Task<Goal?> CreateAsync(CreateGoalDto goalDto) {
        var projectModel = await context.Areas.FindAsync(goalDto.ProjectId);
        
        if (projectModel == null) {
            return null;
        }
        
        var goalModel = goalDto.ToModel();
        await context.Goals.AddAsync(goalModel);
        await context.SaveChangesAsync();
        return goalModel;
    }

    public async Task<Goal?> UpdateAsync(int id, UpdateGoalDto goalDto) {
        var goalModel = await context.Goals.FindAsync(id);
        var projectModel = await context.Areas.FindAsync(goalDto.ProjectId);
        
        if (goalModel == null || projectModel == null) {
            return null;
        }

        goalModel.UpdateModelFromDto(goalDto);
        await context.SaveChangesAsync();
        return goalModel;
    }

    public async Task<Goal?> DeleteAsync(int id) {
        var goalModel = await context.Goals.FindAsync(id);

        if (goalModel == null) {
            return null;
        }

        context.Goals.Remove(goalModel);
        await context.SaveChangesAsync();
        return goalModel;
    }

    public async  Task<bool> GoalExists(int id) {
        return await context.Goals.AnyAsync(a => a.Id == id);
    }
}