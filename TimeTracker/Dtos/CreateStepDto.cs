using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Dtos;

public class CreateStepDto
{
    [Required]
    public int Duration { get; set; }
    [Required]
    public DateOnly CompletedOn { get; set; }
    [MaxLength(500, ErrorMessage = "Content can not be over 500 characters")]
    public string? Description { get; set; }
}