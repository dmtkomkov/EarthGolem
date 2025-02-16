using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Dtos.Step;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;
using TimeTracker.Models;

namespace TimeTracker.Services.Repositories;

public class StepRepository(ApplicationDbContext context) : IStepRepository
{
    public async Task<List<Step>> GetAllAsync(DateOnly? dateFilter) {
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

        return await query
            .OrderByDescending(s => s.Id)
            .ToListAsync();
    }
    
    public async Task<List<StepGroup>> GetAllGroupedByDateAsync() {
        var distinctDates = await context.Steps
            .AsNoTracking()
            .Select(s => s.CompletedOn)
            .Distinct()
            .OrderByDescending(d => d)
            .ToListAsync();
        
        if (distinctDates.Count == 0)
            return [];
        
        var grouped = await context.Steps
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.Category)
                .ThenInclude(c => c.Area)
            .Include(s => s.Goal)
                .ThenInclude(g => g.Project)
            .Where(s => distinctDates.Contains(s.CompletedOn))
            .GroupBy(s => s.CompletedOn)
            .Select(g => new StepGroup {
                CompletedOn = g.Key,
                Steps = g.OrderByDescending(s => s.Id).ToList()
            })
            .OrderByDescending(g => g.CompletedOn)
            .ToListAsync();

        return grouped;
    }


    public async Task<Step?> GetByIdAsync(int id)
    {
        return await context.Steps
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.Category)
                .ThenInclude(c => c.Area)
            .Include(s => s.Goal)
                .ThenInclude(g => g.Project)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Step?> CreateAsync(CreateStepDto stepDto, string userId)
    {
        var stepModel = stepDto.ToModel(userId);
        await context.Steps.AddAsync(stepModel);
        await context.SaveChangesAsync();
        return await context.Steps
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.Category)
                .ThenInclude(c => c.Area)
            .Include(s => s.Goal)
                .ThenInclude(g => g.Project)
            .FirstOrDefaultAsync(s => s.Id == stepModel.Id);
    }

    public async Task<Step?> UpdateAsync(int id, UpdateStepDto stepDto)
    {
        var stepModel = await context.Steps.FindAsync(id);

        if (stepModel == null) {
            return null;
        }

        stepModel.UpdateModelFromDto(stepDto);
        await context.SaveChangesAsync();
        return await context.Steps
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.Category)
                .ThenInclude(c => c.Area)
            .Include(s => s.Goal)
                .ThenInclude(g => g.Project)
            .FirstOrDefaultAsync(s => s.Id == stepModel.Id);
    }

    public async Task<Step?> DeleteAsync(int id)
    {
        var stepModel = await context.Steps.FindAsync(id);

        if (stepModel == null) {
            return null;
        }

        context.Steps.Remove(stepModel);
        await context.SaveChangesAsync();
        return stepModel;
    }

    public Task<bool> StepExists(int id)
    {
        return context.Steps.AnyAsync(s => s.Id == id);
    }
}