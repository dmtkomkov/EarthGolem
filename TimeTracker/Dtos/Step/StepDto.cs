using System.ComponentModel.DataAnnotations;
using TimeTracker.Dtos.User;

namespace TimeTracker.Dtos;

public class StepDto
{
    public int Id { get; set; }
    public int Duration { get; set; }
    public DateOnly CompletedOn { get; set; }
    [MaxLength(500, ErrorMessage = "Content can not be over 500 characters")]
    public string? Description { get; set; }
    [Required]
    public UserDto? User { get; set; }
}