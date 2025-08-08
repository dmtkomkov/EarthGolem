using TimeTracker.Dtos.Project;
using TimeTracker.Models;

namespace TimeTracker.Interfaces;

public interface IProjectRepository {
    Task<List<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(int id);
    Task<Project> CreateAsync(CreateProjectDto projectDto);
    Task<Project?> UpdateAsync(int id, UpdateProjectDto projectDto);
    Task<Project?> DeleteAsync(int id);
    Task<bool> ProjectExists(int id);
    Task UpdateDatesAsync(int? id);
}