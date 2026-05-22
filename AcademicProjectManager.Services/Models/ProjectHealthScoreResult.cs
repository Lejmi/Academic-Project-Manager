namespace AcademicProjectManager.Services.Models;

public class ProjectHealthScoreResult
{
    public int ProjectId { get; set; }
    public string ProjectTitle { get; set; } = string.Empty;
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int OverdueTasks { get; set; }
    public double CompletionRate { get; set; }
    public double HealthScore { get; set; }
}
