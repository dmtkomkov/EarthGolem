using TimeTracker.Dtos;
using TimeTracker.Models;

namespace TimeTracker.Interfaces;

public interface IStepRepository
{
    Task<List<Step>> GetAllAsync();
    Task<Step?> GetByIdAsync(int id);
    Task<Step> CreateAsync(CreateStepDto stepDto, string userId);
    Task<Step?> UpdateAsync(int id, UpdateStepDto stepDto);
    Task<Step?> DeleteAsync(int id);
    Task<bool> StepExists(int id);
}