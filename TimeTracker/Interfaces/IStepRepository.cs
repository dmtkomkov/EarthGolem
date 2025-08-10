using TimeTracker.Dtos.Step;
using TimeTracker.Enums;
using TimeTracker.Models;

namespace TimeTracker.Interfaces;

public interface IStepRepository {
    Task<List<Step>> GetAllAsync(DateOnly? dateFilter, string stepFilter);
    Task<List<StepGroup>> GetAllGroupedByDateAsync(string stepFilter);
    Task<Step?> GetByIdAsync(int id);
    Task<Step?> CreateAsync(CreateStepDto stepDto);
    Task<Step?> UpdateAsync(int id, UpdateStepDto stepDto);
    Task<Step?> ToggleAsync(int id);
    Task<Step?> DeleteAsync(int id);
    Task<bool> StepExists(int id);
}