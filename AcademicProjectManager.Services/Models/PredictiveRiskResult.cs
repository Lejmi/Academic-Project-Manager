namespace AcademicProjectManager.Services.Models;

public class PredictiveRiskResult
{
    public int TaskId { get; set; }
    public string TaskTitle { get; set; } = string.Empty;
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public string ProjectTitle { get; set; } = string.Empty;
    public double RiskFactor { get; set; }
    public string RiskLevel { get; set; } = "Low";
    public string Recommendation { get; set; } = string.Empty;
}
