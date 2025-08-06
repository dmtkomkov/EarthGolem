using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TimeTracker.Models;

public class Step {
    public int Id { get; set; }
    public int Duration { get; set; }
    public DateOnly CompletedOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DateOnly UpdatedOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public bool IsDeleted { get; set; } = false;
    [MaxLength(500)] public string Description { get; set; } = string.Empty;
    [Required] [MaxLength(36)] public required string UserId { get; set; }
    public IdentityUser? User { get; set; }
    [Required] public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public int? GoalId { get; set; }
    public Goal? Goal { get; set; }
}