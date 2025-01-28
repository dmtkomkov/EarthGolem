using TimeTracker.Dtos.Project;
using TimeTracker.Models;

namespace TimeTracker.Mappers;

public static class ProjectMapper {
    public static ProjectDto ToDto(Project projectModel) {
        return new ProjectDto() {
            Id = projectModel.Id,
            Name = projectModel.Name,
            Color = projectModel.Color,
            Description = projectModel.Description,
            StartDate = projectModel.StartDate,
            CloseDate = projectModel.CloseDate,
            Status = projectModel.Status,
            Goals = projectModel.Goals.Select(g => g.ToDto()).ToList(),
        };
    }
}