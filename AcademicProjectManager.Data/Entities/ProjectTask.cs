using System.ComponentModel.DataAnnotations;
using AcademicProjectManager.Data.Enums;

namespace AcademicProjectManager.Data.Entities;

public class ProjectTask
{
    public int Id { get; set; }

    [Required]
    [MaxLength(140)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1200)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime Deadline { get; set; } = DateTime.UtcNow.Date.AddDays(7);

    [Required]
    public TaskWorkflowStatus Status { get; set; } = TaskWorkflowStatus.Pending;

    public bool IsOverdue { get; set; }

    [Range(1, int.MaxValue)]
    public int ProjectId { get; set; }

    public Project? Project { get; set; }

    [Range(1, int.MaxValue)]
    public int AssignedMemberId { get; set; }

    public Member? AssignedMember { get; set; }

    [Required]
    public DateTime AssignedOn { get; set; } = DateTime.UtcNow;

    public DateTime? CompletedOn { get; set; }
}
