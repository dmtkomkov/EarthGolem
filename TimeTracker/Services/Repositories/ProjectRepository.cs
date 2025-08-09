using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Dtos.Project;
using TimeTracker.Enums;
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

    public async Task<Project?> GetByNameAsync(string name) {
        return await context.Projects
            .Include(p => p.Goals)
            .FirstOrDefaultAsync(s => s.Name == name);
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

    public async Task<Project?> ToggleAsync(int id) {
        var projectModel = await context.Projects.FindAsync(id);

        if (projectModel == null) {
            return null;
        }

        projectModel.Status = projectModel.Status == ProjectStatus.Open ? ProjectStatus.Closed : ProjectStatus.Open;
        await context.SaveChangesAsync();

        return projectModel;
    }

    public async Task<bool> ProjectExists(int id) {
        return await context.Projects.AnyAsync(a => a.Id == id);
    }

    public async Task UpdateDatesAsync(int? id) {
        if (!id.HasValue) return;

        var project = await context.Projects.FindAsync(id.Value);
        if (project == null) return;

        var relatedGoals = await context.Goals
            .Where(s => s.ProjectId == id)
            .ToListAsync();

        if (relatedGoals.Count != 0) {
            project.StartDate = relatedGoals.Min(s => s.StartDate);
            project.EndDate = relatedGoals.Max(s => s.EndDate);
        }
        else {
            project.StartDate = null;
            project.EndDate = null;
        }

        await context.SaveChangesAsync();
    }
}