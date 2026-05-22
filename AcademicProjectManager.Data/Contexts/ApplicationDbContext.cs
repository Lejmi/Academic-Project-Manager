using AcademicProjectManager.Data.Entities;
using AcademicProjectManager.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AcademicProjectManager.Data.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectTask> ProjectTasks => Set<ProjectTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Member>()
            .HasMany(m => m.AssignedTasks)
            .WithOne(t => t.AssignedMember)
            .HasForeignKey(t => t.AssignedMemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed Members
        modelBuilder.Entity<Member>().HasData(
            new Member { Id = 1, FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@acme.com", Role = "Project Manager" },
            new Member { Id = 2, FirstName = "Bob", LastName = "Smith", Email = "bob.smith@acme.com", Role = "Developer" },
            new Member { Id = 3, FirstName = "Carol", LastName = "White", Email = "carol.white@acme.com", Role = "QA Engineer" },
            new Member { Id = 4, FirstName = "David", LastName = "Brown", Email = "david.brown@acme.com", Role = "Developer" },
            new Member { Id = 5, FirstName = "Emma", LastName = "Davis", Email = "emma.davis@acme.com", Role = "Designer" }
        );

        // Seed Projects
        var baseDate = new DateTime(2026, 5, 22, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<Project>().HasData(
            new Project
            {
                Id = 1,
                Title = "Mobile App Redesign",
                Description = "Complete redesign of the mobile application for improved UX and performance.",
                StartDate = baseDate.AddDays(-15),
                Deadline = baseDate.AddDays(20),
                IsCompleted = false
            },
            new Project
            {
                Id = 2,
                Title = "Backend API Migration",
                Description = "Migrate legacy backend APIs to microservices architecture.",
                StartDate = baseDate.AddDays(-30),
                Deadline = baseDate.AddDays(45),
                IsCompleted = false
            },
            new Project
            {
                Id = 3,
                Title = "Database Optimization",
                Description = "Performance tuning and query optimization for production database.",
                StartDate = baseDate.AddDays(-5),
                Deadline = baseDate.AddDays(10),
                IsCompleted = false
            }
        );

        // Seed Tasks
        modelBuilder.Entity<ProjectTask>().HasData(
            // Project 1: Mobile App Redesign
            new ProjectTask
            {
                Id = 1,
                Title = "Design mockups for onboarding flow",
                Description = "Create high-fidelity mockups for the new onboarding experience.",
                Deadline = baseDate.AddDays(-5),
                Status = TaskWorkflowStatus.Completed,
                IsOverdue = false,
                ProjectId = 1,
                AssignedMemberId = 5,
                AssignedOn = baseDate.AddDays(-15),
                CompletedOn = baseDate.AddDays(-3)
            },
            new ProjectTask
            {
                Id = 2,
                Title = "Implement UI components",
                Description = "Build React components for redesigned screens.",
                Deadline = baseDate.AddDays(8),
                Status = TaskWorkflowStatus.InProgress,
                IsOverdue = false,
                ProjectId = 1,
                AssignedMemberId = 2,
                AssignedOn = baseDate.AddDays(-10),
                CompletedOn = null
            },
            new ProjectTask
            {
                Id = 3,
                Title = "Integration testing",
                Description = "Test integration between UI and backend services.",
                Deadline = baseDate.AddDays(15),
                Status = TaskWorkflowStatus.Pending,
                IsOverdue = false,
                ProjectId = 1,
                AssignedMemberId = 3,
                AssignedOn = baseDate.AddDays(-5),
                CompletedOn = null
            },
            new ProjectTask
            {
                Id = 4,
                Title = "User acceptance testing",
                Description = "Coordinate with stakeholders for UAT feedback.",
                Deadline = baseDate.AddDays(20),
                Status = TaskWorkflowStatus.Pending,
                IsOverdue = false,
                ProjectId = 1,
                AssignedMemberId = 1,
                AssignedOn = baseDate.AddDays(-2),
                CompletedOn = null
            },
            // Project 2: Backend API Migration
            new ProjectTask
            {
                Id = 5,
                Title = "Architecture design review",
                Description = "Review and finalize microservices architecture.",
                Deadline = baseDate.AddDays(-2),
                Status = TaskWorkflowStatus.Completed,
                IsOverdue = false,
                ProjectId = 2,
                AssignedMemberId = 4,
                AssignedOn = baseDate.AddDays(-30),
                CompletedOn = baseDate.AddDays(-1)
            },
            new ProjectTask
            {
                Id = 6,
                Title = "Implement authentication service",
                Description = "Create new authentication microservice.",
                Deadline = baseDate.AddDays(5),
                Status = TaskWorkflowStatus.InProgress,
                IsOverdue = false,
                ProjectId = 2,
                AssignedMemberId = 2,
                AssignedOn = baseDate.AddDays(-20),
                CompletedOn = null
            },
            new ProjectTask
            {
                Id = 7,
                Title = "Implement payment service",
                Description = "Create new payment processing microservice.",
                Deadline = baseDate.AddDays(10),
                Status = TaskWorkflowStatus.InProgress,
                IsOverdue = false,
                ProjectId = 2,
                AssignedMemberId = 4,
                AssignedOn = baseDate.AddDays(-18),
                CompletedOn = null
            },
            new ProjectTask
            {
                Id = 8,
                Title = "Data migration scripts",
                Description = "Write and test migration scripts for existing data.",
                Deadline = baseDate.AddDays(30),
                Status = TaskWorkflowStatus.Pending,
                IsOverdue = false,
                ProjectId = 2,
                AssignedMemberId = 2,
                AssignedOn = baseDate.AddDays(-10),
                CompletedOn = null
            },
            new ProjectTask
            {
                Id = 9,
                Title = "Load testing and optimization",
                Description = "Conduct load tests and optimize for production scale.",
                Deadline = baseDate.AddDays(40),
                Status = TaskWorkflowStatus.Pending,
                IsOverdue = false,
                ProjectId = 2,
                AssignedMemberId = 4,
                AssignedOn = baseDate.AddDays(-5),
                CompletedOn = null
            },
            // Project 3: Database Optimization
            new ProjectTask
            {
                Id = 10,
                Title = "Index analysis and creation",
                Description = "Analyze query patterns and create optimized indexes.",
                Deadline = baseDate.AddDays(-3),
                Status = TaskWorkflowStatus.Completed,
                IsOverdue = false,
                ProjectId = 3,
                AssignedMemberId = 4,
                AssignedOn = baseDate.AddDays(-5),
                CompletedOn = baseDate.AddDays(-2)
            },
            new ProjectTask
            {
                Id = 11,
                Title = "Query optimization",
                Description = "Rewrite slow queries for better performance.",
                Deadline = baseDate.AddDays(3),
                Status = TaskWorkflowStatus.InProgress,
                IsOverdue = false,
                ProjectId = 3,
                AssignedMemberId = 4,
                AssignedOn = baseDate.AddDays(-3),
                CompletedOn = null
            },
            new ProjectTask
            {
                Id = 12,
                Title = "Performance benchmarking",
                Description = "Benchmark before/after performance metrics.",
                Deadline = baseDate.AddDays(8),
                Status = TaskWorkflowStatus.Pending,
                IsOverdue = false,
                ProjectId = 3,
                AssignedMemberId = 3,
                AssignedOn = baseDate.AddDays(-1),
                CompletedOn = null
            }
        );
    }
}
