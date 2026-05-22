using AcademicProjectManager.Data.Entities;
using AcademicProjectManager.Data.Enums;
using AcademicProjectManager.Services.Models;

namespace AcademicProjectManager.Services.Interfaces;

public interface ITaskService
{
    Task<IReadOnlyList<ProjectTask>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProjectTask>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);
    Task<ProjectTask?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProjectTask> CreateAsync(ProjectTask task, CancellationToken cancellationToken = default);
    Task<ProjectTask?> UpdateAsync(ProjectTask task, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProjectTask>> GetFilteredTasksAsync(string? keyword, string? status, int? projectId, CancellationToken cancellationToken = default);
    Task<int> CalculateDelaysAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PredictiveRiskResult>> GetPredictiveRisksAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateTaskStatusAsync(int taskId, TaskWorkflowStatus status, CancellationToken cancellationToken = default);
}
