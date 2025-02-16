using TimeTracker.Dtos.Step;
using TimeTracker.Models;

namespace TimeTracker.Interfaces;

public interface IStepRepository {
    Task<List<Step>> GetAllAsync(DateOnly? dateFilter);
    Task<List<StepGroup>> GetAllGroupedByDateAsync();
    Task<Step?> GetByIdAsync(int id);
    Task<Step?> CreateAsync(CreateStepDto stepDto, string userId);
    Task<Step?> UpdateAsync(int id, UpdateStepDto stepDto);
    Task<Step?> DeleteAsync(int id);
    Task<bool> StepExists(int id);
}