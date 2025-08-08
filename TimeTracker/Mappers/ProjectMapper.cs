using TimeTracker.Dtos.Project;
using TimeTracker.Models;

namespace TimeTracker.Mappers;

public static class ProjectMapper {
    public static ProjectFlatDto ToFlatDto(this Project projectModel) {
        return new ProjectFlatDto() {
            Id = projectModel.Id,
            Name = projectModel.Name,
            Color = projectModel.Color,
            Description = projectModel.Description,
            StartDate = projectModel.StartDate,
            EndDate = projectModel.EndDate,
            Status = projectModel.Status,
        };
    }

    public static ProjectDto ToDto(this Project projectModel) {
        return new ProjectDto() {
            Id = projectModel.Id,
            Name = projectModel.Name,
            Color = projectModel.Color,
            Description = projectModel.Description,
            StartDate = projectModel.StartDate,
            EndDate = projectModel.EndDate,
            Status = projectModel.Status,
            Goals = projectModel.Goals.Select(g => g.ToFlatDto()).ToList(),
        };
    }

    public static Project ToModel(this CreateProjectDto projectDto) {
        return new Project() {
            Name = projectDto.Name,
            Color = projectDto.Color,
            Description = projectDto.Description,
        };
    }

    public static void UpdateModelFromDto(this Project projectModel, UpdateProjectDto projectDto) {
        projectModel.Name = projectDto.Name;
        projectModel.Color = projectDto.Color;
        projectModel.Description = projectDto.Description;
    }
}