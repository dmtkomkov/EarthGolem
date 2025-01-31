using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Dtos.Project;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;
using TimeTracker.Models;

namespace TimeTracker.Services.Repositories;

public class ProjectRepository(ApplicationDbContext context) : IProjectRepository {
    public async Task<List<Project>> GetAllAsync() {
        return await context.Projects
            .Include(p => p.Goals)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id) {
        return await context.Projects
            .Include(p => p.Goals)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Project> CreateAsync(CreateProjectDto projectDto) {
        var projectModel = projectDto.ToModel();
        await context.Projects.AddAsync(projectModel);
        await context.SaveChangesAsync();
        return projectModel;
    }

    public async Task<Project?> UpdateAsync(int id, UpdateProjectDto projectDto) {
        var projectModel = await context.Projects.FindAsync(id);

        if (projectModel == null) {
            return null;
        }

        projectModel.UpdateModelFromDto(projectDto);
        await context.SaveChangesAsync();
        return projectModel;
    }

    public async Task<Project?> DeleteAsync(int id) {
        var projectModel = await context.Projects.FindAsync(id);

        if (projectModel == null) {
            return null;
        }

        context.Projects.Remove(projectModel);
        await context.SaveChangesAsync();
        return projectModel;
    }

    public async Task<bool> ProjectExists(int id) {
        return await context.Projects.AnyAsync(a => a.Id == id);
    }
}