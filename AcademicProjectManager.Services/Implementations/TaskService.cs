using AcademicProjectManager.Data.Contexts;
using AcademicProjectManager.Data.Entities;
using AcademicProjectManager.Data.Enums;
using AcademicProjectManager.Services.Interfaces;
using AcademicProjectManager.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademicProjectManager.Services.Implementations;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _dbContext;

    public TaskService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ProjectTask>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.ProjectTasks
            .Include(t => t.Project)
            .Include(t => t.AssignedMember)
            .OrderBy(t => t.Deadline)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProjectTask>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ProjectTasks
            .Include(t => t.AssignedMember)
            .Where(t => t.ProjectId == projectId)
            .OrderBy(t => t.Deadline)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProjectTask?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ProjectTasks
            .Include(t => t.Project)
            .Include(t => t.AssignedMember)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<ProjectTask> CreateAsync(ProjectTask task, CancellationToken cancellationToken = default)
    {
        _dbContext.ProjectTasks.Add(task);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await CalculateDelaysAsync(cancellationToken);
        return task;
    }

    public async Task<ProjectTask?> UpdateAsync(ProjectTask task, CancellationToken cancellationToken = default)
    {
        var existingTask = await _dbContext.ProjectTasks.FirstOrDefaultAsync(t => t.Id == task.Id, cancellationToken);
        if (existingTask is null)
        {
            return null;
        }

        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.Deadline = task.Deadline;
        existingTask.Status = task.Status;
        existingTask.AssignedMemberId = task.AssignedMemberId;

        if (existingTask.Status == TaskWorkflowStatus.Completed)
        {
            existingTask.CompletedOn ??= DateTime.UtcNow;
        }

        if (existingTask.Status != TaskWorkflowStatus.Completed)
        {
            existingTask.CompletedOn = null;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        await CalculateDelaysAsync(cancellationToken);
        return existingTask;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var task = await _dbContext.ProjectTasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (task is null)
        {
            return false;
        }

        _dbContext.ProjectTasks.Remove(task);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IReadOnlyList<ProjectTask>> GetFilteredTasksAsync(string? keyword, string? status, int? projectId, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.ProjectTasks
            .Include(t => t.Project)
            .Include(t => t.AssignedMember)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var normalizedKeyword = keyword.Trim().ToLower();
            query = query.Where(t =>
                t.Title.ToLower().Contains(normalizedKeyword) ||
                t.Description.ToLower().Contains(normalizedKeyword));
        }

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<TaskWorkflowStatus>(status, true, out var parsedStatus))
        {
            query = query.Where(t => t.Status == parsedStatus);
        }

        if (projectId.HasValue)
        {
            query = query.Where(t => t.ProjectId == projectId.Value);
        }

        return await query
            .OrderBy(t => t.Deadline)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CalculateDelaysAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var trackedTasks = await _dbContext.ProjectTasks
            .Where(t => t.Status != TaskWorkflowStatus.Completed)
            .ToListAsync(cancellationToken);

        foreach (var task in trackedTasks)
        {
            task.IsOverdue = task.Deadline < now;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return trackedTasks.Count(t => t.IsOverdue);
    }

    public async Task<IReadOnlyList<PredictiveRiskResult>> GetPredictiveRisksAsync(CancellationToken cancellationToken = default)
    {
        var members = await _dbContext.Members
            .Include(m => m.AssignedTasks)
            .ThenInclude(t => t.Project)
            .ToListAsync(cancellationToken);

        var results = new List<PredictiveRiskResult>();

        foreach (var member in members)
        {
            var completedTasks = member.AssignedTasks
                .Where(t => t.Status == TaskWorkflowStatus.Completed && t.CompletedOn.HasValue)
                .ToList();

            var averageDelayDays = completedTasks.Count == 0
                ? 0
                : completedTasks.Average(t => (t.CompletedOn!.Value - t.Deadline).TotalDays);

            var pendingTasks = member.AssignedTasks
                .Where(t => t.Status == TaskWorkflowStatus.Pending || t.Status == TaskWorkflowStatus.InProgress)
                .ToList();

            var utilizationRate = Math.Min(100, pendingTasks.Count * 25);

            foreach (var task in pendingTasks)
            {
                var daysUntilDeadline = (task.Deadline - DateTime.UtcNow).TotalDays;
                var timePressureScore = daysUntilDeadline <= 0 ? 100 : Math.Min(100, 100 / Math.Max(1, daysUntilDeadline));
                var delayBehaviorScore = averageDelayDays <= 0 ? 0 : Math.Min(100, averageDelayDays * 8);
                var riskFactor = Math.Round((utilizationRate * 0.4) + (delayBehaviorScore * 0.35) + (timePressureScore * 0.25), 2);

                var riskLevel = riskFactor switch
                {
                    >= 70 => "High",
                    >= 45 => "Medium",
                    _ => "Low"
                };

                var recommendation = riskLevel == "High"
                    ? $"{member.FirstName} {member.LastName} is overloaded; consider reassigning this task."
                    : "Current assignment appears sustainable.";

                results.Add(new PredictiveRiskResult
                {
                    TaskId = task.Id,
                    TaskTitle = task.Title,
                    MemberId = member.Id,
                    MemberName = $"{member.FirstName} {member.LastName}".Trim(),
                    ProjectId = task.ProjectId,
                    ProjectTitle = task.Project?.Title ?? "Unknown",
                    RiskFactor = riskFactor,
                    RiskLevel = riskLevel,
                    Recommendation = recommendation
                });
            }
        }

        return results
            .OrderByDescending(r => r.RiskFactor)
            .Take(10)
            .ToList();
    }

    public async Task<bool> UpdateTaskStatusAsync(int taskId, TaskWorkflowStatus status, CancellationToken cancellationToken = default)
    {
        var task = await _dbContext.ProjectTasks.FirstOrDefaultAsync(t => t.Id == taskId, cancellationToken);
        if (task is null)
        {
            return false;
        }

        task.Status = status;
        task.CompletedOn = status == TaskWorkflowStatus.Completed ? DateTime.UtcNow : null;

        await _dbContext.SaveChangesAsync(cancellationToken);
        await CalculateDelaysAsync(cancellationToken);
        return true;
    }
}
