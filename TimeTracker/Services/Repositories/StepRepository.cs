using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Dtos.Step;
using TimeTracker.Enums;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;
using TimeTracker.Models;

namespace TimeTracker.Services.Repositories;

public class StepRepository(ApplicationDbContext context, IGoalRepository goalRepository) : IStepRepository {
    public async Task<List<Step>> GetAllAsync(DateOnly? dateFilter, string stepFilter) {
        IQueryable<Step> query = context.Steps
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.Category)
            .ThenInclude(c => c.Area)
            .Include(s => s.Goal)
            .ThenInclude(g => g.Project);

        if (dateFilter.HasValue) {
            query = query.Where(s => s.CompletedOn == dateFilter.Value);
        }

        query = stepFilter switch {
            StepParam.Active => query.Where(s => !s.IsDeleted),
            StepParam.Deleted => query.Where(s => s.IsDeleted),
            _ => query
        };

        return await query
            .OrderByDescending(s => s.Id)
            .ToListAsync();
    }

    public async Task<List<StepGroup>> GetAllGroupedByDateAsync(string stepFilter) {
        var distinctDates = await context.Steps
            .AsNoTracking()
            .Select(s => s.CompletedOn)
            .Distinct()
            .OrderByDescending(d => d)
            .ToListAsync();

        if (distinctDates.Count == 0)
            return [];

        IQueryable<Step> query = context.Steps
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.Category)
            .ThenInclude(c => c.Area)
            .Include(s => s.Goal)
            .ThenInclude(g => g.Project);
        
        query = stepFilter switch {
            StepParam.Active => query.Where(s => !s.IsDeleted),
            StepParam.Deleted => query.Where(s => s.IsDeleted),
            _ => query
        };
        
        return await query
            .Where(s => distinctDates.Contains(s.CompletedOn))
            .GroupBy(s => s.CompletedOn)
            .Select(g => new StepGroup {
                CompletedOn = g.Key,
                Steps = g.OrderByDescending(s => s.Id).ToList()
            })
            .OrderByDescending(g => g.CompletedOn)
            .ToListAsync();
    }


    public async Task<Step?> GetByIdAsync(int id) {
        return await context.Steps
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.Category)
            .ThenInclude(c => c!.Area)
            .Include(s => s.Goal)
            .ThenInclude(g => g!.Project)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Step?> CreateAsync(CreateStepDto stepDto) {
        var stepModel = stepDto.ToModel();

        await using var transaction = await context.Database.BeginTransactionAsync();

        try {
            await context.Steps.AddAsync(stepModel);
            await context.SaveChangesAsync();
            await goalRepository.UpdateDatesAsync(stepModel.GoalId);

            await transaction.CommitAsync();
        }
        catch {
            await transaction.RollbackAsync();
            throw;
        }

        return await context.Steps
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.Category)
            .ThenInclude(c => c.Area)
            .Include(s => s.Goal)
            .ThenInclude(g => g.Project)
            .FirstOrDefaultAsync(s => s.Id == stepModel.Id);
    }

    public async Task<Step?> UpdateAsync(int id, UpdateStepDto stepDto) {
        var stepModel = await context.Steps.FindAsync(id);

        if (stepModel == null) {
            return null;
        }

        await using var transaction = await context.Database.BeginTransactionAsync();

        try {
            stepModel.UpdateModelFromDto(stepDto);
            await context.SaveChangesAsync();
            await goalRepository.UpdateDatesAsync(stepModel.GoalId);

            await transaction.CommitAsync();
        }
        catch {
            await transaction.RollbackAsync();
            throw;
        }

        return await context.Steps
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.Category)
            .ThenInclude(c => c.Area)
            .Include(s => s.Goal)
            .ThenInclude(g => g.Project)
            .FirstOrDefaultAsync(s => s.Id == stepModel.Id);
    }

    public async Task<Step?> ToggleAsync(int id) {
        var stepModel = await context.Steps
            .Include(s => s.User)
            .Include(s => s.Category)
                .ThenInclude(c => c.Area)
            .Include(s => s.Goal)
                .ThenInclude(g => g.Project)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (stepModel == null) {
            return null;
        }

        await using var transaction = await context.Database.BeginTransactionAsync();

        try {
            stepModel.IsDeleted = !stepModel.IsDeleted;
            stepModel.UpdatedOn = DateOnly.FromDateTime(DateTime.Today);
            await context.SaveChangesAsync();
            await goalRepository.UpdateDatesAsync(stepModel.GoalId);

            await transaction.CommitAsync();
        }
        catch {
            await transaction.RollbackAsync();
            throw;
        }

        return stepModel;
    }

    public async Task<Step?> DeleteAsync(int id) {
        var stepModel = await context.Steps.FindAsync(id);

        if (stepModel == null) {
            return null;
        }

        await using var transaction = await context.Database.BeginTransactionAsync();

        try {
            context.Steps.Remove(stepModel);
            await context.SaveChangesAsync();
            await goalRepository.UpdateDatesAsync(stepModel.GoalId);

            await transaction.CommitAsync();
        }
        catch {
            await transaction.RollbackAsync();
            throw;
        }

        return stepModel;
    }

    public Task<bool> StepExists(int id) {
        return context.Steps.AnyAsync(s => s.Id == id);
    }
}