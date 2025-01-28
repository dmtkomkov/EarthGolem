using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace TimeTracker.Dtos;

public class UpdateStepDto
{
    [Required]
    public int Duration { get; set; }
    [Required]
    public DateOnly CompletedOn { get; set; }
    [MaxLength(500, ErrorMessage = "Content can not be over 500 characters")]
    public string Description { get; set; } = string.Empty;
    [Required]
    public string? UserId { get; set; }
}