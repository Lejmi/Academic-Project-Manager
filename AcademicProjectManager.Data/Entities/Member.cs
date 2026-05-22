using System.ComponentModel.DataAnnotations;

namespace AcademicProjectManager.Data.Entities;

public class Member
{
    public int Id { get; set; }

    [Required]
    [MaxLength(60)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(60)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(120)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(60)]
    public string Role { get; set; } = string.Empty;

    public ICollection<ProjectTask> AssignedTasks { get; set; } = new List<ProjectTask>();
}
