using AcademicProjectManager.Data.Entities;
using AcademicProjectManager.Services.Models;

namespace AcademicProjectManager.Services.Interfaces;

public interface IMemberService
{
    Task<IReadOnlyList<Member>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Member?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Member> CreateAsync(Member member, CancellationToken cancellationToken = default);
    Task<Member?> UpdateAsync(Member member, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<MemberPerformanceResult> GetMemberPerformanceAsync(int memberId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MemberPerformanceResult>> GetAllMemberPerformanceAsync(CancellationToken cancellationToken = default);
}
