using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TimeTracker.Models;

public class Post {
    public int Id { get; set; }
    [MaxLength(255)] public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    [Required] [MaxLength(36)] public required string UserId { get; set; }
    public IdentityUser? User { get; set; }
}
