using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Dtos.Goal;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;
using TimeTracker.Models;

namespace TimeTracker.Services.Repositories;

public class GoalRepository(ApplicationDbContext context) : IGoalRepository {
    public async Task<List<Goal>> GetAllAsync() {
        return await context.Goals
            .Include(g => g.Project)
            .ToListAsync();
    }

    public async Task<List<GoalGroup>> GetAllGroupedByProjectAsync() {
        var projects = await context.Projects
            .AsNoTracking()
            .OrderByDescending(p => p.Id)
            .Select(p => p.Name)
            .ToListAsync();
        
        var groupedGoals = await context.Goals
            .AsNoTracking()
            .Include(g => g.Project)
            .Where(g => g.Project == null || projects.Contains(g.Project!.Name))
            .GroupBy(g => g.Project)
            .Select(g => new GoalGroup() {
                Project = g.Key,
                Goals = g.OrderByDescending(g => g.Id).ToList()
            })
            .OrderByDescending(g => g.Project != null ? g.Project.Id : 0)
            .ToListAsync();

        return groupedGoals;
    }
    
    public async Task<Goal?> GetByIdAsync(int id) {
        return await context.Goals
            .Include(g => g.Project)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Goal?> CreateAsync(CreateGoalDto goalDto) {
        var goalModel = goalDto.ToModel();
        await context.Goals.AddAsync(goalModel);
        await context.SaveChangesAsync();
        return goalModel;
    }

    public async Task<Goal?> UpdateAsync(int id, UpdateGoalDto goalDto) {
        var goalModel = await context.Goals.FindAsync(id);
        
        if (goalModel == null) {
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