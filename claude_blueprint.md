# LLM System Blueprint: Academic Project Management & Analytics Platform

## Project Overview
This blueprint provides detailed, step-by-step instructions to build a complete Web Application using .NET (Blazor + Entity Framework Core). The domain is an Academic Project Management system featuring deep analytical capabilities, automatic delay detection, and a clean 3-tier architecture.

## Tech Stack & Core Rules
- **Framework**: .NET 8 (or latest) Blazor Web Application.
- **ORM**: Entity Framework Core (Code-First approach with Migrations).
- **Architecture**: Strict 3-Layer separation:
  1. `Data` Layer (Entities, DbContext, Repositories/Interfaces).
  2. `Services` Layer (Business logic, LINQ queries, delay detection).
  3. `UI` Layer (Blazor components, Pages, ViewModels).
- **Coding Standards**: 
  - 100% Asynchronous operations (`async/await`, `Task<T>`) across all layers.
  - Form validations must strictly use Data Annotations on the models.
  - Queries must leverage LINQ for dynamic filtering and searching.

---

## Phase 1: Solution Setup & Layered Architecture
**Objective**: Establish the foundation and boilerplate.
1. Create a new blank solution named `AcademicProjectManager`.
2. Add three separate projects to the solution:
   - `AcademicProjectManager.Data` (Class Library)
   - `AcademicProjectManager.Services` (Class Library, references Data)
   - `AcademicProjectManager.UI` (Blazor Web App, references Services & Data)
3. Install required NuGet packages in the Data layer: `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Tools`.
4. Register dependency injection (DI) containers in the `Program.cs` of the UI project.

## Phase 2: Domain Modeling & Entity Framework Setup
**Objective**: Create the Code-First models and generate the initial database.
1. Define the following core entities in the `Data` project. Include strict Data Annotations (`[Required]`, `[MaxLength]`, `[Range]`) for validation:
   - `Member`: Id, FirstName, LastName, Email, Role.
   - `Project`: Id, Title, Description, StartDate, Deadline, IsCompleted. Navigation property to `Tasks`.
   - `ProjectTask` (avoid `Task` keyword conflict): Id, Title, Description, Deadline, Status (Pending, InProgress, Completed), ProjectId, AssignedMemberId.
2. Create the `ApplicationDbContext` class inheriting from `DbContext`. Add `DbSet` properties for the entities.
3. Configure the database connection string in `appsettings.json` and register the `DbContext` in `Program.cs`.
4. Run EF Core Code-First commands: `Add-Migration InitialCreate` and `Update-Database`.

## Phase 3: Data Access & Core Business Services
**Objective**: Implement CRUD operations and advanced LINQ-based analytical services.
1. **Base CRUD Services**: Create interfaces and service classes (e.g., `IProjectService`, `ITaskService`, `IMemberService`) to handle asynchronous CRUD operations.
2. **Dynamic Search & Filtering (LINQ)**:
   - Implement a method `GetFilteredTasksAsync(string keyword, string status, int? projectId)`. Use LINQ `Where` clauses dynamically based on provided parameters.
3. **Delay Detection Engine**:
   - Create a background service or a dedicated method `CalculateDelaysAsync()`.
   - Logic: Any `ProjectTask` where `Deadline < DateTime.Now` and `Status != Completed` must be flagged as `IsOverdue`.
4. **Analytical Calculation Service**:
   - `GetProjectHealthScoreAsync(int projectId)`:
     - Calculate `% of Tasks Completed`: `(Completed Tasks / Total Tasks) * 100`.
     - Count `Overdue Tasks`.
     - Calculate `Global Health Score`: A weighted metric (e.g., 100 points minus points for each overdue task, plus points for completion rate).
   - `GetMemberPerformanceAsync(int memberId)`:
     - Calculate `Completion Rate`: `(Completed Tasks by Member / Total Assigned Tasks) * 100`.
     - Calculate `Workload`: Count of active tasks currently assigned.

## Phase 4: Blazor UI - CRUD Pages & Forms
**Objective**: Build the user-facing forms and tables.
1. **Layout & Navigation**: Setup a clean navigation menu (Sidebar) routing to Projects, Members, and Dashboard.
2. **Project & Member Management**:
   - Implement `Projects.razor` and `Members.razor` list views.
   - Implement `EditProject.razor` using `<EditForm>`, `<DataAnnotationsValidator>`, and `<ValidationSummary>`.
3. **Task Assignment**:
   - Inside a Project detail page, allow adding a `ProjectTask`. Include a dropdown to select and assign a `Member`.
   - Ensure UI updates asynchronously when a task's status changes.

## Phase 5: The Analytical Dashboard
**Objective**: Fulfill the core visual requirements with charts and indicators.
1. Create `Dashboard.razor` as the homepage.
2. Integrate a Blazor charting library (e.g., MudBlazor, Radzen, or Chart.js via JSInterop).
3. **Project Health Widgets**:
   - Display a list or grid of active projects.
   - For each, show a Progress Bar (`% Tasks Completed`).
   - Show a Badge/Alert for `Tasks in Delay`.
   - Display the `Health Score` as a gauge chart or color-coded metric (Green > 80, Yellow > 50, Red < 50).
4. **Member Performance Metrics**:
   - Display a Bar Chart comparing the `Completion Rate` of different members.
   - Display a table highlighting members with the highest `Workload` (for resource management).

## Phase 6: Extension & Creativity Step (Advanced Analytics)
**Objective**: Fulfill the "2 points creativity" requirement with a coherent, high-value data-driven extension.
1. **Feature to Implement**: *Predictive Delay Warning System (Data-Driven)*.
2. **Logic**: Instead of just detecting *current* delays, calculate a `Risk Factor` for pending tasks based on historical performance.
   - Add a method to evaluate a member's historical average completion time vs. task duration.
   - If a member consistently finishes similar tasks late, flag new tasks assigned to them with a `High Risk of Delay` tag *before* the deadline hits.
3. **UI Integration**: On the Dashboard, add a "Predictive Risk" panel that suggests workload rebalancing (e.g., "Member X has a 90% utilization rate and historical delay patterns; suggest moving Task Y to Member Z"). This perfectly aligns with data analysis and decision-making logic.

## Phase 7: Final Polish & Export
1. Ensure strict separation of concerns (Blazor UI must not reference EF Core directly, only the Services layer).
2. Clean up UI elements (CSS/Bootstrap) for a professional look.
3. Document the codebase inline so the final PDF report generation is straightforward.
