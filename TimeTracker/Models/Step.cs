using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TimeTracker.Models;

public class Step
{
    public int Id { get; set; }
    public int Duration { get; set; }
    public DateOnly CompletedOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    [MaxLength(36)]
    public string? UserId { get; set; }
    public IdentityUser? User { get; set; }
}