using System.ComponentModel.DataAnnotations;
using System.Drawing;
using TimeTracker.Dtos.Project;

namespace TimeTracker.Dtos.Goal;

public class GoalFlatDto {
    public int Id { get; set; }
    [MaxLength(36)] [Required] public string Name { get; set; } = string.Empty;
    [MaxLength(500, ErrorMessage = "Content can not be over 500 characters")]
    public string Description { get; set; } = string.Empty;
    public DateOnly? StartDate { get; set; }
    public DateOnly? CloseDate { get; set; }
    public int? ProjectId { get; set; }
}