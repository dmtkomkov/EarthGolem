using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using TimeTracker.Enums;

namespace TimeTracker.Models;

public class Goal {
    public int Id { get; set; }
    [MaxLength(36)] public string Name { get; set; } = string.Empty;
    [MaxLength(500)] public string Description { get; set; } = string.Empty;
    public DateOnly? StartDate { get; set; }
    [Required] public GoalStatus Status { get; set; } = GoalStatus.Open;
    public DateOnly? EndDate { get; set; }
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
    public List<Step> Steps { get; set; } = [];

    public int StepCount => Steps?.Count(s => !s.IsDeleted) ?? 0;
}