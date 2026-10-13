using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Dtos.Post;

public class UpdatePostDto {
    [MaxLength(255)] [Required] public string Title { get; set; } = string.Empty;
    [Required] public string Body { get; set; } = string.Empty;
}
