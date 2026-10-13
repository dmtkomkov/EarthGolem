using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Dtos.Post;

public class CreatePostDto {
    [MaxLength(255)] [Required] public string Title { get; set; } = string.Empty;
    [Required] public string Body { get; set; } = string.Empty;
}
