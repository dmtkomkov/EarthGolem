﻿namespace TimeTracker.Models;

public class Step
{
    public int Id { get; set; }
    public int Duration { get; set; }
    public DateOnly CompletedOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string? Description { get; set; } = null;
}