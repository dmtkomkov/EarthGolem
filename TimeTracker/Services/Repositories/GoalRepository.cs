using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Dtos.Goal;
using TimeTracker.Enums;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;
using TimeTracker.Models;

namespace TimeTracker.Services.Repositories;

public class GoalRepository(ApplicationDbContext context, IProjectRepository projectRepository) : IGoalRepository {
    public async Task<List<Goal>> GetAllAsync(string? projectFilter) {
        IQueryable<Goal> query = context.Goals
            .Include(g => g.Project)
            .Include(g => g.Steps);

        if (!string.IsNullOrWhiteSpace(projectFilter)) {
            if (projectFilter.Equals("null", StringComparison.OrdinalIgnoreCase)) {
                query = query.Where(g => g.Project == null);
            }
            else {
                query = query.Where(g => g.Project != null && g.Project.Name == projectFilter);
            }
        }

        return await query
            .OrderByDescending(g => g.Id)
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
        await using var transaction = await context.Database.BeginTransactionAsync();

        try {
            if (goalDto.Project is not null) {
                var projectModel = goalDto.Project.ToModel();
                await context.Projects.AddAsync(projectModel);
                await context.SaveChangesAsync();
                // Set project id in goal
                goalDto.ProjectId = projectModel.Id;
            }

            var goalModel = goalDto.ToModel();

            await context.Goals.AddAsync(goalModel);
            await context.SaveChangesAsync();
            await projectRepository.UpdateDatesAsync(goalModel.ProjectId);

            await transaction.CommitAsync();

            return goalModel;
        }
        catch (Exception ex) {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Goal?> UpdateAsync(int id, UpdateGoalDto goalDto) {
        var goalModel = await context.Goals.FindAsync(id);

        if (goalModel == null) {
            return null;
        }

        await using var transaction = await context.Database.BeginTransactionAsync();

        try {
            goalModel.UpdateModelFromDto(goalDto);
            await context.SaveChangesAsync();
            await projectRepository.UpdateDatesAsync(goalModel.ProjectId);

            await transaction.CommitAsync();
        }
        catch {
            await transaction.RollbackAsync();
            throw;
        }

        return goalModel;
    }

    public async Task<Goal?> DeleteAsync(int id) {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try {
            var goalModel = await context.Goals
                .Include(g => g.Project)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (goalModel == null) {
                return null;
            }

            var projectModel = goalModel.Project;

            context.Goals.Remove(goalModel);
            await context.SaveChangesAsync();

            if (projectModel != null) {
                var hasOtherGoals = await context.Goals.AnyAsync(g => g.ProjectId == projectModel.Id);

                if (!hasOtherGoals) {
                    context.Projects.Remove(projectModel);
                    await context.SaveChangesAsync();
                }
                else {
                    await projectRepository.UpdateDatesAsync(goalModel.ProjectId);
                }
            }

            await transaction.CommitAsync();

            return goalModel;
        }
        catch (Exception ex) {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Goal?> ToggleAsync(int id) {
        var goalModel = await context.Goals.FindAsync(id);

        if (goalModel == null) {
            return null;
        }

        goalModel.Status = goalModel.Status == GoalStatus.Open ? GoalStatus.Closed : GoalStatus.Open;
        await context.SaveChangesAsync();

        return goalModel;
    }

    public async Task<bool> GoalExists(int id) {
        return await context.Goals.AnyAsync(a => a.Id == id);
    }

    public async Task UpdateDatesAsync(int? id) {
        if (!id.HasValue) return;

        var goalModel = await context.Goals.FindAsync(id.Value);
        if (goalModel == null) return;

        var relatedSteps = await context.Steps
            .Where(s => s.GoalId == id && !s.IsDeleted)
            .ToListAsync();

        if (relatedSteps.Count != 0) {
            goalModel.StartDate = relatedSteps.Min(s => s.CompletedOn);
            goalModel.EndDate = relatedSteps.Max(s => s.CompletedOn);
        }
        else {
            goalModel.StartDate = null;
            goalModel.EndDate = null;
        }

        await projectRepository.UpdateDatesAsync(goalModel.ProjectId);

        await context.SaveChangesAsync();
    }
}