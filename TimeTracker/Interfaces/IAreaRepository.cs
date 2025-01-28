using TimeTracker.Dtos.Area;
using TimeTracker.Models;

namespace TimeTracker.Interfaces;

public interface IAreaRepository {
    Task<List<Area>> GetAllAsync();
    Task<Area?> GetByIdAsync(int id);
    Task<Area> CreateAsync(CreateAreaDto areaDto);
    Task<Area?> UpdateAsync(int id, UpdateAreaDto areaDto);
    Task<Area?> DeleteAsync(int id);
    Task<bool> AreaExists(int id);
}