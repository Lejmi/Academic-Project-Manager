using System.ComponentModel.DataAnnotations;

namespace AcademicProjectManager.Data.Entities;

public class Project
{
    public int Id { get; set; }

    [Required]
    [MaxLength(140)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1200)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;

    [Required]
    public DateTime Deadline { get; set; } = DateTime.UtcNow.Date.AddDays(30);

    public bool IsCompleted { get; set; }

    public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
}
