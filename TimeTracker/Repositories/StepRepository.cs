﻿using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Dtos;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;
using TimeTracker.Models;

namespace TimeTracker.Repositories;

public class StepRepository(ApplicationDbContext context) : IStepRepository
{
    public async Task<List<Step>> GetAllAsync()
    {
        return await context.Steps.ToListAsync();
    }

    public async Task<Step?> GetByIdAsync(int id)
    {
        return await context.Steps.FindAsync(id);
    }

    public async Task<Step> CreateAsync(CreateStepDto stepDto)
    {
        var stepModel = stepDto.ToModel();
        await context.Steps.AddAsync(stepModel);
        await context.SaveChangesAsync();
        return stepModel;
    }

    public async Task<Step?> UpdateAsync(int id, UpdateStepDto stepDto)
    {
        var stepModel = await context.Steps.FindAsync(id);

        if (stepModel == null)
        {
            return null;
        }

        stepModel.UpdateModelFromDto(stepDto);
        await context.SaveChangesAsync();

        return stepModel;
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