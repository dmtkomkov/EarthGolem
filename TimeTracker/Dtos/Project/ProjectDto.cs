using System.ComponentModel.DataAnnotations;
using System.Drawing;
using TimeTracker.Dtos.Goal;
using TimeTracker.Enums;
using TimeTracker.Models;

namespace TimeTracker.Dtos.Project;

public class ProjectDto : ProjectFlatDto {
    [Required] public List<GoalFlatDto> Goals { get; set; } = [];
}