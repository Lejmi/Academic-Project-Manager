using AcademicProjectManager.Data.Contexts;
using AcademicProjectManager.Data.Entities;
using AcademicProjectManager.Data.Enums;
using AcademicProjectManager.Services.Interfaces;
using AcademicProjectManager.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademicProjectManager.Services.Implementations;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _dbContext;

    public ProjectService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Projects
            .Include(p => p.Tasks)
            .OrderBy(p => p.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Projects
            .Include(p => p.Tasks)
            .ThenInclude(t => t.AssignedMember)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Project> CreateAsync(Project project, CancellationToken cancellationToken = default)
    {
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return project;
    }

    public async Task<Project?> UpdateAsync(Project project, CancellationToken cancellationToken = default)
    {
        var existingProject = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == project.Id, cancellationToken);
        if (existingProject is null)
        {
            return null;
        }

        existingProject.Title = project.Title;
        existingProject.Description = project.Description;
        existingProject.StartDate = project.StartDate;
        existingProject.Deadline = project.Deadline;
        existingProject.IsCompleted = project.IsCompleted;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return existingProject;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (project is null)
        {
            return false;
        }

        _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<ProjectHealthScoreResult> GetProjectHealthScoreAsync(int projectId, CancellationToken cancellationToken = default)
    {
        var project = await _dbContext.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);

        if (project is null)
        {
            return new ProjectHealthScoreResult
            {
                ProjectId = projectId,
                ProjectTitle = "Unknown",
                HealthScore = 0
            };
        }

        var totalTasks = project.Tasks.Count;
        var completedTasks = project.Tasks.Count(t => t.Status == TaskWorkflowStatus.Completed);
        var overdueTasks = project.Tasks.Count(t => t.IsOverdue);
        var completionRate = totalTasks == 0 ? 0 : completedTasks * 100.0 / totalTasks;

        var healthScore = Math.Clamp(completionRate - (overdueTasks * 12), 0, 100);

        return new ProjectHealthScoreResult
        {
            ProjectId = project.Id,
            ProjectTitle = project.Title,
            TotalTasks = totalTasks,
            CompletedTasks = completedTasks,
            OverdueTasks = overdueTasks,
            CompletionRate = Math.Round(completionRate, 2),
            HealthScore = Math.Round(healthScore, 2)
        };
    }

    public async Task<IReadOnlyList<ProjectHealthScoreResult>> GetAllProjectHealthScoresAsync(CancellationToken cancellationToken = default)
    {
        var projects = await _dbContext.Projects
            .Include(p => p.Tasks)
            .OrderBy(p => p.Title)
            .ToListAsync(cancellationToken);

        var results = projects.Select(project =>
        {
            var totalTasks = project.Tasks.Count;
            var completedTasks = project.Tasks.Count(t => t.Status == TaskWorkflowStatus.Completed);
            var overdueTasks = project.Tasks.Count(t => t.IsOverdue);
            var completionRate = totalTasks == 0 ? 0 : completedTasks * 100.0 / totalTasks;
            var healthScore = Math.Clamp(completionRate - (overdueTasks * 12), 0, 100);

            return new ProjectHealthScoreResult
            {
                ProjectId = project.Id,
                ProjectTitle = project.Title,
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                OverdueTasks = overdueTasks,
                CompletionRate = Math.Round(completionRate, 2),
                HealthScore = Math.Round(healthScore, 2)
            };
        }).ToList();

        return results;
    }
}
