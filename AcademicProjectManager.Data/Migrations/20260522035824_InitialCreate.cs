using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AcademicProjectManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 140, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1200, nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Deadline = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 140, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1200, nullable: false),
                    Deadline = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    IsOverdue = table.Column<bool>(type: "INTEGER", nullable: false),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignedMemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompletedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTasks_Members_AssignedMemberId",
                        column: x => x.AssignedMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Role" },
                values: new object[,]
                {
                    { 1, "alice.johnson@acme.com", "Alice", "Johnson", "Project Manager" },
                    { 2, "bob.smith@acme.com", "Bob", "Smith", "Developer" },
                    { 3, "carol.white@acme.com", "Carol", "White", "QA Engineer" },
                    { 4, "david.brown@acme.com", "David", "Brown", "Developer" },
                    { 5, "emma.davis@acme.com", "Emma", "Davis", "Designer" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Deadline", "Description", "IsCompleted", "StartDate", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), "Complete redesign of the mobile application for improved UX and performance.", false, new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Mobile App Redesign" },
                    { 2, new DateTime(2026, 7, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Migrate legacy backend APIs to microservices architecture.", false, new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Backend API Migration" },
                    { 3, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Performance tuning and query optimization for production database.", false, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Database Optimization" }
                });

            migrationBuilder.InsertData(
                table: "ProjectTasks",
                columns: new[] { "Id", "AssignedMemberId", "AssignedOn", "CompletedOn", "Deadline", "Description", "IsOverdue", "ProjectId", "Status", "Title" },
                values: new object[,]
                {
                    { 1, 5, new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Create high-fidelity mockups for the new onboarding experience.", false, 1, 3, "Design mockups for onboarding flow" },
                    { 2, 2, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Build React components for redesigned screens.", false, 1, 2, "Implement UI components" },
                    { 3, 3, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Test integration between UI and backend services.", false, 1, 1, "Integration testing" },
                    { 4, 1, new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), "Coordinate with stakeholders for UAT feedback.", false, 1, 1, "User acceptance testing" },
                    { 5, 4, new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Review and finalize microservices architecture.", false, 2, 3, "Architecture design review" },
                    { 6, 2, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 5, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Create new authentication microservice.", false, 2, 2, "Implement authentication service" },
                    { 7, 4, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Create new payment processing microservice.", false, 2, 2, "Implement payment service" },
                    { 8, 2, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), "Write and test migration scripts for existing data.", false, 2, 1, "Data migration scripts" },
                    { 9, 4, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Conduct load tests and optimize for production scale.", false, 2, 1, "Load testing and optimization" },
                    { 10, 4, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "Analyze query patterns and create optimized indexes.", false, 3, 3, "Index analysis and creation" },
                    { 11, 4, new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Rewrite slow queries for better performance.", false, 3, 2, "Query optimization" },
                    { 12, 3, new DateTime(2026, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Benchmark before/after performance metrics.", false, 3, 1, "Performance benchmarking" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_AssignedMemberId",
                table: "ProjectTasks",
                column: "AssignedMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ProjectId",
                table: "ProjectTasks",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTasks");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
