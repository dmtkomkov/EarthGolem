using System.ComponentModel.DataAnnotations;
using System.Drawing;
using TimeTracker.Dtos.Goal;
using TimeTracker.Enums;
using TimeTracker.Models;

namespace TimeTracker.Dtos.Project;

public class ProjectDto {
    public int Id { get; set; }
    [MaxLength(36)]
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public Color Color { get; set; }
    [MaxLength(500, ErrorMessage = "Content can not be over 500 characters")]
    public string Description { get; set; } = string.Empty;
    [Required]
    public ProjectStatus Status { get; set; } = ProjectStatus.Active;
    public DateOnly? StartDate { get; set; }
    public DateOnly? CloseDate { get; set; }
    [Required]
    public List<GoalDto> Goals { get; set; } = [];
}