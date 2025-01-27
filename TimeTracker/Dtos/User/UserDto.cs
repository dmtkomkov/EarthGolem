using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Dtos.User;

public class UserDto {
    [Required]
    public string? UserId { get; set; }
    [Required]
    public string? UserName { get; set; }
}