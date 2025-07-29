using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Dtos.User;

public class UserDto {
    [Required]
    public required string UserId { get; set; }
    [Required]
    public required string? UserName { get; set; }
}