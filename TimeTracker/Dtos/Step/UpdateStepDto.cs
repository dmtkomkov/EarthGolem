using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Dtos.Step;

public class UpdateStepDto {
    [Required] public int Duration { get; set; }
    [Required] public DateOnly CompletedOn { get; set; }

    [MaxLength(500, ErrorMessage = "Content can not be over 500 characters")]
    public string Description { get; set; } = string.Empty;

    [Required] public required string UserId { get; set; }
    public int CategoryId { get; set; }
    public int? GoalId { get; set; }
}