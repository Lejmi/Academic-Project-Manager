using AcademicProjectManager.Data.Entities;
using AcademicProjectManager.Services.Models;

namespace AcademicProjectManager.Services.Interfaces;

public interface IProjectService
{
    Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Project> CreateAsync(Project project, CancellationToken cancellationToken = default);
    Task<Project?> UpdateAsync(Project project, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ProjectHealthScoreResult> GetProjectHealthScoreAsync(int projectId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProjectHealthScoreResult>> GetAllProjectHealthScoresAsync(CancellationToken cancellationToken = default);
}
