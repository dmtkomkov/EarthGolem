using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Dtos.Step;

public class CreateStepDto {
    public string? UserId { get; set; }
    [Required] public int Duration { get; set; }
    [Required] public DateOnly CompletedOn { get; set; }

    [MaxLength(500, ErrorMessage = "Content can not be over 500 characters")]
    public string Description { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public int? GoalId { get; set; }
}