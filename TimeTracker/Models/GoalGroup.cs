namespace TimeTracker.Models;

public class GoalGroup {
    public Project? Project { get; set; }
    public List<Goal> Goals { get; set; } = [];
}