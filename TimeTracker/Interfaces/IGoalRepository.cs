using TimeTracker.Dtos.Category;
using TimeTracker.Dtos.Goal;
using TimeTracker.Models;

namespace TimeTracker.Interfaces;

public interface IGoalRepository {
    Task<List<Goal>> GetAllAsync();
    Task<Goal?> GetByIdAsync(int id);
    Task<Goal> CreateAsync(CreateGoalDto goalDto);
    Task<Goal?> UpdateAsync(int id, UpdateGoalDto goalDto);
    Task<Goal?> DeleteAsync(int id);
    Task<bool> GoalExists(int id);
}