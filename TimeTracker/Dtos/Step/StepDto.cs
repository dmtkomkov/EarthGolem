using System.ComponentModel.DataAnnotations;
using TimeTracker.Dtos.Category;
using TimeTracker.Dtos.Goal;
using TimeTracker.Dtos.User;

namespace TimeTracker.Dtos.Step;

public class StepDto
{
    public int Id { get; set; }
    [Required]
    public int Duration { get; set; }
    [Required]
    public DateOnly CompletedOn { get; set; }
    public DateOnly UpdatedOn { get; set; }
    public bool IsDeleted { get; set; }
    [MaxLength(500, ErrorMessage = "Content can not be over 500 characters")]
    public string Description { get; set; } = string.Empty;
    public required UserDto User { get; set; }
    public required CategoryDto Category { get; set; }
    public GoalDto? Goal { get; set; }
}