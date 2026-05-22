namespace AcademicProjectManager.Services.Models;

public class MemberPerformanceResult
{
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public int TotalAssignedTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int Workload { get; set; }
    public double CompletionRate { get; set; }
    public double AverageDelayDays { get; set; }
    public double UtilizationRate { get; set; }
}
