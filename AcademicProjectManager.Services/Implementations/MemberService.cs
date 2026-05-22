using AcademicProjectManager.Data.Contexts;
using AcademicProjectManager.Data.Entities;
using AcademicProjectManager.Data.Enums;
using AcademicProjectManager.Services.Interfaces;
using AcademicProjectManager.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademicProjectManager.Services.Implementations;

public class MemberService : IMemberService
{
    private readonly ApplicationDbContext _dbContext;

    public MemberService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Member>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Members
            .OrderBy(m => m.FirstName)
            .ThenBy(m => m.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Member?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Members
            .Include(m => m.AssignedTasks)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<Member> CreateAsync(Member member, CancellationToken cancellationToken = default)
    {
        _dbContext.Members.Add(member);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return member;
    }

    public async Task<Member?> UpdateAsync(Member member, CancellationToken cancellationToken = default)
    {
        var existingMember = await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == member.Id, cancellationToken);
        if (existingMember is null)
        {
            return null;
        }

        existingMember.FirstName = member.FirstName;
        existingMember.LastName = member.LastName;
        existingMember.Email = member.Email;
        existingMember.Role = member.Role;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return existingMember;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var member = await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        if (member is null)
        {
            return false;
        }

        _dbContext.Members.Remove(member);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<MemberPerformanceResult> GetMemberPerformanceAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var member = await _dbContext.Members
            .Include(m => m.AssignedTasks)
            .ThenInclude(t => t.Project)
            .FirstOrDefaultAsync(m => m.Id == memberId, cancellationToken);

        if (member is null)
        {
            return new MemberPerformanceResult
            {
                MemberId = memberId,
                MemberName = "Unknown"
            };
        }

        return BuildMemberPerformance(member);
    }

    public async Task<IReadOnlyList<MemberPerformanceResult>> GetAllMemberPerformanceAsync(CancellationToken cancellationToken = default)
    {
        var members = await _dbContext.Members
            .Include(m => m.AssignedTasks)
            .ThenInclude(t => t.Project)
            .OrderBy(m => m.FirstName)
            .ThenBy(m => m.LastName)
            .ToListAsync(cancellationToken);

        return members.Select(BuildMemberPerformance).ToList();
    }

    private static MemberPerformanceResult BuildMemberPerformance(Member member)
    {
        var totalAssigned = member.AssignedTasks.Count;
        var completed = member.AssignedTasks.Count(t => t.Status == TaskWorkflowStatus.Completed);
        var workload = member.AssignedTasks.Count(t => t.Status == TaskWorkflowStatus.Pending || t.Status == TaskWorkflowStatus.InProgress);
        var completionRate = totalAssigned == 0 ? 0 : completed * 100.0 / totalAssigned;

        var finishedTasks = member.AssignedTasks
            .Where(t => t.Status == TaskWorkflowStatus.Completed && t.CompletedOn.HasValue)
            .ToList();

        var averageDelayDays = finishedTasks.Count == 0
            ? 0
            : finishedTasks.Average(t => (t.CompletedOn!.Value - t.Deadline).TotalDays);

        var utilizationRate = Math.Min(100, workload * 25);

        return new MemberPerformanceResult
        {
            MemberId = member.Id,
            MemberName = $"{member.FirstName} {member.LastName}".Trim(),
            TotalAssignedTasks = totalAssigned,
            CompletedTasks = completed,
            Workload = workload,
            CompletionRate = Math.Round(completionRate, 2),
            AverageDelayDays = Math.Round(averageDelayDays, 2),
            UtilizationRate = utilizationRate
        };
    }
}
