# Academic Project Manager - Complete Code Walkthrough

## Overview

The Academic Project Manager is a web application that helps teams manage projects and tasks. If you're seeing this code for the first time, this guide will walk you through how it works from start to finish.

Think of it like this: **Users interact with web pages → Pages talk to Services → Services read/write to Database**.

---

## Table of Contents

1. [Project Structure](#project-structure)
2. [The Three-Layer Architecture](#the-three-layer-architecture)
3. [How Data Flows Through the System](#how-data-flows-through-the-system)
4. [The Database Layer](#the-database-layer)
5. [The Service Layer](#the-service-layer)
6. [The UI Layer](#the-ui-layer)
7. [Common Operations Walkthrough](#common-operations-walkthrough)
8. [Key Concepts](#key-concepts)

---

## Project Structure

The solution has 3 projects organized like this:

```
AcademicProjectManager.slnx (Solution file)
├── AcademicProjectManager.Data/          (Database & Entities)
│   ├── Contexts/
│   │   └── ApplicationDbContext.cs        (Database configuration & seed data)
│   ├── Entities/
│   │   ├── Member.cs                      (Team member entity)
│   │   ├── Project.cs                     (Project entity)
│   │   └── ProjectTask.cs                 (Task entity)
│   ├── Enums/
│   │   └── TaskWorkflowStatus.cs          (Task status enum)
│   └── Migrations/
│       └── (Database migration files)
│
├── AcademicProjectManager.Services/      (Business Logic)
│   ├── Interfaces/
│   │   ├── IProjectService.cs
│   │   ├── IMemberService.cs
│   │   └── ITaskService.cs
│   ├── Implementations/
│   │   ├── ProjectService.cs              (Project business logic)
│   │   ├── MemberService.cs               (Member business logic)
│   │   └── TaskService.cs                 (Task business logic)
│   └── Models/
│       ├── MemberPerformanceResult.cs
│       ├── ProjectHealthScoreResult.cs
│       └── PredictiveRiskResult.cs
│
└── AcademicProjectManager.UI/            (Web Pages & Forms)
    ├── Components/
    │   └── Pages/
    │       ├── Home.razor                 (Dashboard)
    │       ├── Projects.razor             (Projects list)
    │       ├── EditProject.razor          (Create/Edit project)
    │       ├── ProjectDetails.razor       (Project detail view + tasks)
    │       ├── Members.razor              (Members list)
    │       └── EditMember.razor           (Create/Edit member)
    ├── Properties/
    │   └── launchSettings.json            (App configuration)
    └── Program.cs                         (App startup configuration)
```

---

## The Three-Layer Architecture

This project uses a **three-layer architecture**. Each layer has a specific job:

### Layer 1: Data Layer (`AcademicProjectManager.Data`)

**Purpose:** Store and retrieve data from the database

**What it does:**
- Defines what data looks like (Entities)
- Manages the database connection (DbContext)
- Stores the seed data (initial sample data)
- Handles database migrations

**Key Files:**
- `ApplicationDbContext.cs` - The database connection manager
- `Member.cs`, `Project.cs`, `ProjectTask.cs` - The data models

### Layer 2: Service Layer (`AcademicProjectManager.Services`)

**Purpose:** Business logic and calculations

**What it does:**
- Implements rules like "calculate completion rate"
- Fetches data from the database via queries
- Transforms raw database data into useful information
- Validates operations (e.g., "can this task be deleted?")

**Key Files:**
- `ProjectService.cs` - "How do I get projects and calculate their health?"
- `MemberService.cs` - "What are my team member's stats?"
- `TaskService.cs` - "What's the risk level for this task?"

### Layer 3: UI Layer (`AcademicProjectManager.UI`)

**Purpose:** What users see and interact with

**What it does:**
- Shows project lists, member lists, dashboards
- Handles user interactions (button clicks, form submissions)
- Calls services to get/save data
- Displays data on web pages

**Key Files:**
- `Home.razor` - The dashboard users see first
- `EditProject.razor` - The form to create/edit projects
- `ProjectDetails.razor` - The page showing a single project's tasks

---

## How Data Flows Through the System

Here's the complete journey of data:

### Example: User Creates a New Project

```
1. USER INTERACTION (UI Layer)
   └─ User fills out form on EditProject.razor
      └─ User clicks "Save" button

2. FORM SUBMISSION (UI Layer → Service Layer)
   └─ EditProject.razor calls ProjectService.CreateAsync(newProject)
      └─ Passes the project data

3. VALIDATION & PROCESSING (Service Layer)
   └─ ProjectService.CreateAsync() validates the data
      └─ Calls _dbContext.Projects.Add(project)
         └─ Queues the project for database insertion

4. DATABASE WRITE (Service Layer → Data Layer)
   └─ Calls await _dbContext.SaveChangesAsync()
      └─ SQLite database receives the INSERT command
         └─ Database writes the new project to disk

5. RESPONSE (Service Layer → UI Layer)
   └─ Returns the created project back to UI
      └─ UI redirects to Projects list page
         └─ User sees the new project in the list

6. DISPLAY (UI Layer)
   └─ Projects.razor queries the list of all projects
      └─ ServiceLayer returns the list
         └─ Page renders with new project visible
```

---

## The Database Layer

### Understanding the Database (`ApplicationDbContext.cs`)

The database context is like a bridge between C# code and the SQLite database. Here's what's in it:

```csharp
public class ApplicationDbContext : DbContext
{
    // These DbSet properties represent tables in the database
    public DbSet<Member> Members => Set<Member>();      // Members table
    public DbSet<Project> Projects => Set<Project>();    // Projects table
    public DbSet<ProjectTask> ProjectTasks => Set<ProjectTask>(); // Tasks table
}
```

### The Three Entities

#### 1. Member (Team Member)

```csharp
public class Member
{
    public int Id { get; set; }                    // Primary key
    public string FirstName { get; set; }          // First name
    public string LastName { get; set; }           // Last name
    public string Email { get; set; }              // Email address
    public string Role { get; set; }               // Job role (Developer, Manager, etc)
    public ICollection<ProjectTask> AssignedTasks { get; set; } // All tasks assigned to this member
}
```

**What it represents:** A person on your team

**Real example:**
```
Id: 1
FirstName: "Alice"
LastName: "Johnson"
Email: "alice@company.com"
Role: "Project Manager"
AssignedTasks: [Task 1, Task 2, Task 3]  // All tasks assigned to Alice
```

#### 2. Project

```csharp
public class Project
{
    public int Id { get; set; }                     // Primary key
    public string Title { get; set; }               // Project name
    public string Description { get; set; }         // What is this project about?
    public DateTime StartDate { get; set; }         // When did it start?
    public DateTime Deadline { get; set; }          // When is it due?
    public bool IsCompleted { get; set; }           // Is it finished?
    public ICollection<ProjectTask> Tasks { get; set; } // All tasks in this project
}
```

**What it represents:** A project your team is working on

**Real example:**
```
Id: 1
Title: "Mobile App Redesign"
Description: "Complete redesign of the mobile application for improved UX"
StartDate: May 7, 2026
Deadline: June 11, 2026
IsCompleted: false
Tasks: [Task 1, Task 2, Task 3, Task 4]  // All tasks for this project
```

#### 3. ProjectTask (A single task)

```csharp
public class ProjectTask
{
    public int Id { get; set; }                     // Primary key
    public string Title { get; set; }               // Task name
    public string Description { get; set; }         // What needs to be done?
    public DateTime Deadline { get; set; }          // When is it due?
    public TaskWorkflowStatus Status { get; set; }  // Pending, InProgress, or Completed
    public DateTime? CompletedOn { get; set; }      // When was it finished?
    public bool IsOverdue { get; set; }             // Is it past deadline?
    public int ProjectId { get; set; }              // Which project? (Foreign key)
    public Project Project { get; set; }            // The project this belongs to
    public int? AssignedMemberId { get; set; }      // Who is doing this? (Foreign key)
    public Member AssignedMember { get; set; }      // The person assigned
    public DateTime AssignedOn { get; set; }        // When was it assigned?
}
```

**What it represents:** A single unit of work within a project

**Real example:**
```
Id: 5
Title: "Design mockups for onboarding flow"
Description: "Create high-fidelity mockups for the new onboarding experience"
Deadline: May 17, 2026
Status: Completed                           // Task is done
CompletedOn: May 17, 2026                  // Finished on this date
IsOverdue: false                            // Finished on time
ProjectId: 1                                // Part of "Mobile App Redesign"
AssignedMemberId: 5                         // Assigned to member with id 5 (Emma Davis)
AssignedOn: May 7, 2026                    // Assigned on this date
```

### How the Relationships Work

**Think of it like this:**

```
One Project can have Many Tasks
│
└─ Mobile App Redesign (Project)
   ├─ Task 1: Design mockups
   ├─ Task 2: Implement UI components
   ├─ Task 3: Integration testing
   └─ Task 4: User acceptance testing

One Member can have Many Tasks
│
└─ Alice Johnson (Member)
   ├─ Assigned to Task 1
   ├─ Assigned to Task 3
   └─ Assigned to Task 5
```

### Seed Data

When the application starts, it automatically populates the database with sample data:

- **5 Members:** Alice Johnson, Bob Smith, Carol White, David Brown, Emma Davis
- **3 Projects:** Mobile App Redesign, Backend API Migration, Database Optimization
- **12 Tasks:** Distributed across projects with various statuses

This allows you to start using the app immediately without creating data manually.

---

## The Service Layer

Services are where the business logic lives. They answer questions like:

- "What's the completion rate for member Alice?"
- "Is this project healthy or at risk?"
- "Which tasks might be delayed?"

### ProjectService (`ProjectService.cs`)

**What it does:** Manages all project-related operations

**Key Methods:**

```csharp
// Get all projects
public async Task<IReadOnlyList<Project>> GetAllAsync()
{
    return await _dbContext.Projects
        .Include(p => p.Tasks)  // Also get all tasks for each project
        .OrderBy(p => p.Title)
        .ToListAsync();
}
// Why Include()? So we get the project AND its tasks in one query

// Get one specific project
public async Task<Project?> GetByIdAsync(int id)
{
    return await _dbContext.Projects
        .Include(p => p.Tasks)  // Get tasks too
        .FirstOrDefaultAsync(p => p.Id == id);
}

// Calculate project health score
public async Task<double> GetProjectHealthScoreAsync(int projectId)
{
    var project = await GetByIdAsync(projectId);
    
    // Count how many tasks are done
    var completed = project.Tasks.Count(t => t.Status == TaskWorkflowStatus.Completed);
    var total = project.Tasks.Count;
    
    // Calculate completion percentage
    var completionRate = total == 0 ? 0 : (completed * 100.0 / total);
    
    // Count overdue tasks
    var overdueTasks = project.Tasks.Count(t => t.IsOverdue && t.Status != TaskWorkflowStatus.Completed);
    
    // Health = Completion Rate - (Overdue Tasks × 12%)
    // Example: 80% complete - (2 overdue tasks × 12%) = 80 - 24 = 56 health
    var healthScore = completionRate - (overdueTasks * 12);
    
    // Keep it between 0 and 100
    return Math.Clamp(healthScore, 0, 100);
}
```

### MemberService (`MemberService.cs`)

**What it does:** Manages team member data and performance tracking

**Key Methods:**

```csharp
// Get all team members
public async Task<IReadOnlyList<Member>> GetAllAsync()
{
    return await _dbContext.Members
        .OrderBy(m => m.FirstName)
        .ThenBy(m => m.LastName)
        .ToListAsync();
}

// Calculate member performance
public async Task<IReadOnlyList<MemberPerformanceResult>> GetAllMemberPerformanceAsync()
{
    var members = await _dbContext.Members
        .Include(m => m.AssignedTasks)  // Get all tasks for each member
        .ToListAsync();
    
    var results = new List<MemberPerformanceResult>();
    
    foreach (var member in members)
    {
        // Count total tasks assigned to this member
        var totalAssigned = member.AssignedTasks.Count;
        
        // Count how many are completed
        var completed = member.AssignedTasks
            .Count(t => t.Status == TaskWorkflowStatus.Completed);
        
        // Calculate completion rate: (Completed / Total) × 100
        // Example: 3 completed out of 10 total = 30%
        var completionRate = totalAssigned == 0 ? 0 : (completed * 100.0 / totalAssigned);
        
        results.Add(new MemberPerformanceResult
        {
            MemberId = member.Id,
            MemberName = member.FirstName + " " + member.LastName,
            CompletedTasks = completed,
            TotalAssignedTasks = totalAssigned,
            CompletionRate = Math.Round(completionRate, 2)
        });
    }
    
    return results;
}
```

### TaskService (`TaskService.cs`)

**What it does:** Manages tasks and predicts which ones might be delayed

**Key Methods:**

```csharp
// Create a new task
public async Task<ProjectTask> CreateAsync(ProjectTask task)
{
    // Add the task to the database
    _dbContext.ProjectTasks.Add(task);
    
    // Save changes
    await _dbContext.SaveChangesAsync();
    
    // Update overdue status for all tasks
    await CalculateDelaysAsync();
    
    return task;
}

// Mark tasks as overdue if they're past their deadline
public async Task<int> CalculateDelaysAsync()
{
    var now = DateTime.UtcNow;  // Current time
    
    // Get all incomplete tasks
    var trackedTasks = await _dbContext.ProjectTasks
        .Where(t => t.Status != TaskWorkflowStatus.Completed)
        .ToListAsync();
    
    // Check which ones are past their deadline
    foreach (var task in trackedTasks)
    {
        task.IsOverdue = task.Deadline < now;  // Compare deadline to now
    }
    
    await _dbContext.SaveChangesAsync();
    return trackedTasks.Count(t => t.IsOverdue);
}

// Update task status (Pending → InProgress → Completed)
public async Task<bool> UpdateTaskStatusAsync(int taskId, TaskWorkflowStatus status)
{
    var task = await _dbContext.ProjectTasks
        .FirstOrDefaultAsync(t => t.Id == taskId);
    
    if (task is null)
        return false;  // Task not found
    
    task.Status = status;
    
    // If marking as completed, record the completion time
    if (status == TaskWorkflowStatus.Completed)
    {
        task.CompletedOn = DateTime.UtcNow;  // Record when it was completed
    }
    else
    {
        task.CompletedOn = null;  // Clear completion time if unmarking
    }
    
    await _dbContext.SaveChangesAsync();
    return true;
}

// Predict which tasks are at risk of being delayed
public async Task<IReadOnlyList<PredictiveRiskResult>> GetPredictiveRisksAsync()
{
    var results = new List<PredictiveRiskResult>();
    
    // Get all members with their tasks
    var members = await _dbContext.Members
        .Include(m => m.AssignedTasks)
        .ToListAsync();
    
    foreach (var member in members)
    {
        // Get tasks this member is still working on
        var pendingTasks = member.AssignedTasks
            .Where(t => t.Status == TaskWorkflowStatus.Pending || 
                        t.Status == TaskWorkflowStatus.InProgress)
            .ToList();
        
        // Check each pending task for risk
        foreach (var task in pendingTasks)
        {
            // 1. How many tasks does this member have? (Workload)
            var workload = pendingTasks.Count;
            var utilizationRate = Math.Min(100, workload * 25);  // 25% per task
            
            // 2. Has this member been late on previous tasks? (Track record)
            var completedTasks = member.AssignedTasks
                .Where(t => t.Status == TaskWorkflowStatus.Completed && 
                           t.CompletedOn.HasValue)
                .ToList();
            
            var averageDelayDays = completedTasks.Count == 0 ? 0 :
                completedTasks.Average(t => 
                    (t.CompletedOn.Value - t.Deadline).TotalDays);
            
            var delayBehaviorScore = averageDelayDays <= 0 ? 0 : 
                Math.Min(100, averageDelayDays * 8);
            
            // 3. How much time is left until deadline? (Time pressure)
            var daysUntilDeadline = (task.Deadline - DateTime.UtcNow).TotalDays;
            var timePressureScore = daysUntilDeadline <= 0 ? 100 : 
                Math.Min(100, 100 / Math.Max(1, daysUntilDeadline));
            
            // Combine all factors: 40% workload, 35% history, 25% time pressure
            var riskFactor = (utilizationRate * 0.4) + 
                           (delayBehaviorScore * 0.35) + 
                           (timePressureScore * 0.25);
            
            // Classify as High/Medium/Low risk
            var riskLevel = riskFactor >= 70 ? "High" : 
                           riskFactor >= 45 ? "Medium" : "Low";
            
            results.Add(new PredictiveRiskResult
            {
                TaskId = task.Id,
                TaskTitle = task.Title,
                MemberId = member.Id,
                MemberName = member.FirstName + " " + member.LastName,
                RiskFactor = riskFactor,
                RiskLevel = riskLevel
            });
        }
    }
    
    return results;
}
```

---

## The UI Layer

The UI layer is where users interact with the application. It's built with **Blazor**, which is a framework for building interactive web applications with C#.

### Key Concept: Blazor Components (`.razor` files)

A Blazor component is like an HTML page combined with C# code. It:
- Displays data
- Handles user interactions
- Calls services to get/save data
- Updates what's shown on screen

### Example: EditProject.razor (Create/Edit a Project)

```csharp
@page "/projects/edit/{Id:int?}"
@rendermode InteractiveServer
@inject IProjectService ProjectService
@inject NavigationManager NavigationManager

<div class="card">
    <h3>@(Id == null ? "Create" : "Edit") Project</h3>
    
    <form @onsubmit="HandleSaveClick">
        <!-- Project Title -->
        <div>
            <label>Title</label>
            <input type="text" 
                   value="@_title" 
                   @oninput="@((ChangeEventArgs e) => _title = (string?)e.Value ?? "")" />
        </div>
        
        <!-- Project Description -->
        <div>
            <label>Description</label>
            <textarea @oninput="@((ChangeEventArgs e) => _description = (string?)e.Value ?? "")" 
                      rows="4">@_description</textarea>
        </div>
        
        <!-- Start Date -->
        <div>
            <label>Start Date</label>
            <input type="date" 
                   value="@_startDate.ToString("yyyy-MM-dd")" 
                   @onchange="@((ChangeEventArgs e) => {
                       if (DateTime.TryParse((string?)e.Value, out var dt))
                           _startDate = dt;
                   })" />
        </div>
        
        <!-- Deadline -->
        <div>
            <label>Deadline</label>
            <input type="date" 
                   value="@_deadline.ToString("yyyy-MM-dd")" 
                   @onchange="@((ChangeEventArgs e) => {
                       if (DateTime.TryParse((string?)e.Value, out var dt))
                           _deadline = dt;
                   })" />
        </div>
        
        <button type="submit">Save Project</button>
    </form>
</div>

@code {
    // Page parameter (from URL: /projects/edit/5)
    [Parameter]
    public int? Id { get; set; }
    
    // Local state (data in memory for this page)
    private string _title = "";
    private string _description = "";
    private DateTime _startDate = DateTime.Today;
    private DateTime _deadline = DateTime.Today.AddDays(14);
    
    // Lifecycle: Called when page first loads
    protected override async Task OnInitializedAsync()
    {
        if (Id.HasValue)  // Editing existing project
        {
            var project = await ProjectService.GetByIdAsync(Id.Value);
            _title = project.Title;
            _description = project.Description;
            _startDate = project.StartDate;
            _deadline = project.Deadline;
        }
    }
    
    // Handles form submission (Save button clicked)
    private async Task HandleSaveClick()
    {
        // Validate
        if (string.IsNullOrWhiteSpace(_title))
        {
            // Show error
            return;
        }
        
        if (Id.HasValue)  // Update existing
        {
            var project = new Project
            {
                Id = Id.Value,
                Title = _title,
                Description = _description,
                StartDate = _startDate,
                Deadline = _deadline
            };
            await ProjectService.UpdateAsync(project);
        }
        else  // Create new
        {
            var project = new Project
            {
                Title = _title,
                Description = _description,
                StartDate = _startDate,
                Deadline = _deadline
            };
            await ProjectService.CreateAsync(project);
        }
        
        // Navigate back to projects list
        NavigationManager.NavigateTo("/projects");
    }
}
```

### Understanding Event Handlers

When you see `@oninput` and `@onchange`, these are listening for user interactions:

```csharp
// When user types in a text box
@oninput="@((ChangeEventArgs e) => _title = (string?)e.Value ?? "")"
         └─ Calls this code immediately as user types
             └─ Updates _title variable with new value
                 └─ Page re-renders automatically

// When user changes a date
@onchange="@((ChangeEventArgs e) => {
    if (DateTime.TryParse((string?)e.Value, out var dt))
        _startDate = dt;
})"
```

### The Dashboard (Home.razor)

The dashboard displays all the important metrics:

```csharp
@page "/"
@rendermode InteractiveServer
@inject IProjectService ProjectService
@inject IMemberService MemberService
@inject ITaskService TaskService

<h1>Academic Project Analytics Dashboard</h1>

<!-- KPIs: Key Performance Indicators -->
<div class="kpi-grid">
    <div class="kpi-card">
        <h6>Active Projects</h6>
        <!-- Count projects that aren't 100% complete -->
        <h2>@_projectScores.Count(x => x.HealthScore < 100)</h2>
    </div>
    
    <div class="kpi-card">
        <h6>Delayed Tasks</h6>
        <!-- Count all overdue tasks -->
        <h2>@_projectScores.Sum(x => x.OverdueTasks)</h2>
    </div>
    
    <div class="kpi-card">
        <h6>Avg. Health Score</h6>
        <!-- Average health of all projects -->
        <h2>@(_projectScores.Average(x => x.HealthScore))%</h2>
    </div>
</div>

<!-- Project Health Overview -->
<div class="card">
    <h4>Project Health Overview</h4>
    @foreach (var project in _projectScores)
    {
        <div>
            <strong>@project.ProjectTitle</strong>
            <div class="progress-bar" style="width: @project.CompletionRate%;"></div>
            <small>@project.CompletionRate% complete | @project.OverdueTasks overdue</small>
        </div>
    }
</div>

<!-- Member Performance -->
<div class="card">
    <h4>Member Completion Rate</h4>
    <table>
        <tr>
            <th>Member</th>
            <th>Completed</th>
            <th>Rate</th>
        </tr>
        @foreach (var member in _memberPerformance)
        {
            <tr>
                <td>@member.MemberName</td>
                <td>@member.CompletedTasks / @member.TotalAssignedTasks</td>
                <td>
                    <div class="progress-bar" style="width: @member.CompletionRate%;"></div>
                    @member.CompletionRate%
                </td>
            </tr>
        }
    </table>
</div>

<!-- Predictive Risk Warning -->
<div class="card">
    <h4>Predictive Delay Warning</h4>
    @foreach (var risk in _predictiveRisks)
    {
        <div class="risk-item">
            <strong>@risk.TaskTitle</strong>
            <span class="risk-level @risk.RiskLevel">@risk.RiskLevel (@risk.RiskFactor%)</span>
            <small>@risk.MemberName in @risk.ProjectTitle</small>
            <p>@risk.Recommendation</p>
        </div>
    }
</div>

@code {
    private IReadOnlyList<ProjectHealthScoreResult> _projectScores = [];
    private IReadOnlyList<MemberPerformanceResult> _memberPerformance = [];
    private IReadOnlyList<PredictiveRiskResult> _predictiveRisks = [];
    
    protected override async Task OnInitializedAsync()
    {
        // Called when page loads - fetch all data
        await RefreshAsync();
    }
    
    private async Task RefreshAsync()
    {
        // Update which tasks are overdue
        await TaskService.CalculateDelaysAsync();
        
        // Get project health data
        _projectScores = await ProjectService.GetAllProjectHealthScoresAsync();
        
        // Get member performance data
        _memberPerformance = await MemberService.GetAllMemberPerformanceAsync();
        
        // Get risk predictions
        _predictiveRisks = (await TaskService.GetPredictiveRisksAsync()).Take(5).ToList();
        
        // Tell Blazor to re-render the page with new data
        StateHasChanged();
    }
}
```

---

## Common Operations Walkthrough

Let's trace through complete operations to see how all layers work together.

### Operation 1: Creating a New Project

**Step 1: User Interface**

User navigates to `/projects/create` and sees the EditProject form. They enter:
- Title: "API Security Audit"
- Description: "Review and harden all API endpoints"
- Start Date: May 22, 2026
- Deadline: June 5, 2026

User clicks "Save" button.

**Step 2: Form Submission (UI → Service)**

EditProject.razor's `HandleSaveClick()` is triggered:

```csharp
private async Task HandleSaveClick()
{
    // Create Project object with user's input
    var project = new Project
    {
        Title = "API Security Audit",
        Description = "Review and harden all API endpoints",
        StartDate = DateTime(2026, 5, 22),
        Deadline = DateTime(2026, 6, 5),
        IsCompleted = false
    };
    
    // Call service
    await ProjectService.CreateAsync(project);
    
    // Navigate back
    NavigationManager.NavigateTo("/projects");
}
```

**Step 3: Service Processing (Service → Database)**

ProjectService.CreateAsync() runs:

```csharp
public async Task<Project> CreateAsync(Project project)
{
    // Add project to DbContext (queue for database)
    _dbContext.Projects.Add(project);
    
    // Actually write to database
    await _dbContext.SaveChangesAsync();
    
    // Return the project (now has an Id assigned by database)
    return project;  // project.Id is now 4 (auto-incremented)
}
```

**Step 4: Database Write (Data Layer)**

SQLite receives an INSERT command:

```sql
INSERT INTO Projects (Title, Description, StartDate, Deadline, IsCompleted)
VALUES ('API Security Audit', 'Review and harden all API endpoints', 
        '2026-05-22', '2026-06-05', 0)
RETURNING Id;
```

SQLite writes the new row to the database file and returns the auto-generated Id (4).

**Step 5: Navigation & Display (UI)**

EditProject.razor navigates to `/projects` which loads Projects.razor:

```csharp
// Projects.razor calls
_projects = await ProjectService.GetAllAsync();

// This returns ALL projects including the new one:
[
    { Id: 1, Title: "Mobile App Redesign", ... },
    { Id: 2, Title: "Backend API Migration", ... },
    { Id: 3, Title: "Database Optimization", ... },
    { Id: 4, Title: "API Security Audit", ... }  // New project!
]
```

The page renders with the new project visible in the list.

---

### Operation 2: Updating a Task Status

**Step 1: User Interaction**

User is viewing ProjectDetails page for Project 1. They see a task:
- Title: "Design mockups"
- Current Status: InProgress

User clicks the status dropdown and selects "Completed".

**Step 2: Status Update (UI → Service)**

ProjectDetails.razor's status change handler is triggered:

```csharp
// User changed dropdown to "Completed"
private async Task UpdateTaskStatusAsync(int taskId, TaskWorkflowStatus newStatus)
{
    // newStatus = TaskWorkflowStatus.Completed
    await TaskService.UpdateTaskStatusAsync(taskId, newStatus);
    
    // Refresh the page to show updated status
    await LoadProjectAsync();
}
```

**Step 3: Service Processing (Service → Database)**

TaskService.UpdateTaskStatusAsync() runs:

```csharp
public async Task<bool> UpdateTaskStatusAsync(int taskId, TaskWorkflowStatus status)
{
    // Get the task from database
    var task = await _dbContext.ProjectTasks
        .FirstOrDefaultAsync(t => t.Id == taskId);
    
    // Update its status
    task.Status = TaskWorkflowStatus.Completed;
    
    // Record when it was completed
    if (status == TaskWorkflowStatus.Completed)
    {
        task.CompletedOn = DateTime.UtcNow;  // May 22, 2026, 14:30:00
    }
    
    // Save to database
    await _dbContext.SaveChangesAsync();
    
    // Recalculate which tasks are overdue (this task is now done, so no longer overdue)
    await CalculateDelaysAsync();
    
    return true;
}
```

**Step 4: Database Update (Data Layer)**

SQLite receives an UPDATE command:

```sql
UPDATE ProjectTasks 
SET Status = 3,  -- 3 = Completed enum value
    CompletedOn = '2026-05-22 14:30:00'
WHERE Id = 1;
```

**Step 5: Recalculation (Service)**

CalculateDelaysAsync() runs:

```csharp
public async Task<int> CalculateDelaysAsync()
{
    var now = DateTime.UtcNow;
    var trackedTasks = await _dbContext.ProjectTasks
        .Where(t => t.Status != TaskWorkflowStatus.Completed)  // Exclude completed
        .ToListAsync();
    
    // For all INCOMPLETE tasks, check if they're past deadline
    foreach (var task in trackedTasks)
    {
        task.IsOverdue = task.Deadline < now;
    }
    
    await _dbContext.SaveChangesAsync();
    return trackedTasks.Count(t => t.IsOverdue);
}
```

**Step 6: Display Update (UI)**

ProjectDetails.razor re-renders showing:
- Task status changed from "InProgress" to "Completed" ✓
- Task no longer shows as overdue (since it's completed)
- Project completion percentage increases (1 more task done)
- Project health score recalculates

---

### Operation 3: Dashboard Displays Predictive Risk

**Step 1: Page Load**

User navigates to Home dashboard (`/`)

**Step 2: Data Fetching (UI → Service)**

Home.razor's OnInitializedAsync() runs:

```csharp
protected override async Task OnInitializedAsync()
{
    // Calculate current overdue status
    await TaskService.CalculateDelaysAsync();
    
    // Get all project health
    _projectScores = await ProjectService.GetAllProjectHealthScoresAsync();
    
    // Get all member performance
    _memberPerformance = await MemberService.GetAllMemberPerformanceAsync();
    
    // GET PREDICTIVE RISKS <-- This is the important one
    _predictiveRisks = (await TaskService.GetPredictiveRisksAsync()).Take(5).ToList();
    
    StateHasChanged();  // Re-render with data
}
```

**Step 3: Risk Analysis (Service)**

TaskService.GetPredictiveRisksAsync() analyzes each pending task:

For a task assigned to Bob Smith:
```
Task: "Implement payment service"
Assigned to: Bob Smith
Status: InProgress
Deadline: May 29, 2026 (7 days from now)

Analysis:
1. Workload = 3 active tasks → Utilization = 75%
2. Bob's history = averages 2 days late on past tasks → Delay score = 16
3. Time until deadline = 7 days → Time pressure = 14

Risk Factor = (75 × 0.4) + (16 × 0.35) + (14 × 0.25)
            = 30 + 5.6 + 3.5
            = 39.1 (Medium Risk)

Recommendation: "Current assignment appears sustainable."
```

For another task assigned to Alice:
```
Task: "Load testing and optimization"
Assigned to: Alice
Status: InProgress
Deadline: June 1, 2026 (10 days away)
But Alice has 5 active tasks and averages 4 days late...

Risk Factor = (125 × 0.4) + (32 × 0.35) + (10 × 0.25)
            = 50 + 11.2 + 2.5
            = 63.7 (High Risk)

Recommendation: "Alice Johnson is overloaded; consider reassigning this task."
```

**Step 4: Display (UI)**

Home.razor renders the "Predictive Delay Warning" section:

```
Task: Implement payment service
Risk: Medium (39.1%)
Member: Bob Smith in Backend API Migration
Recommendation: Current assignment appears sustainable.

Task: Load testing and optimization
Risk: High (63.7%)
Member: Alice Johnson in Backend API Migration
Recommendation: Alice Johnson is overloaded; consider reassigning this task.
```

---

## Key Concepts

### 1. Async/Await

All database operations use `async` and `await`:

```csharp
// This doesn't block - allows other requests to be processed
public async Task<Project> GetByIdAsync(int id)
{
    // await waits for database, but doesn't freeze the application
    return await _dbContext.Projects
        .FirstOrDefaultAsync(p => p.Id == id);
}
```

**Why it matters:** Your web app can handle multiple users simultaneously without freezing.

### 2. Entity Framework Core (LINQ)

LINQ is a way to write database queries in C# instead of SQL:

```csharp
// This LINQ query:
var completedTasks = member.AssignedTasks
    .Where(t => t.Status == TaskWorkflowStatus.Completed)
    .ToList();

// Translates to this SQL:
SELECT * FROM ProjectTasks 
WHERE Status = 3 AND AssignedMemberId = @memberId;
```

### 3. Dependency Injection

Services are "injected" into components, so they don't have to create them:

```csharp
// In EditProject.razor
@inject IProjectService ProjectService

// Now you can call:
await ProjectService.CreateAsync(project);
```

**Why it matters:** Easy to test, swap implementations, and manage the lifetime of services.

### 4. Interfaces

Each service has an interface (contract) that defines what it does:

```csharp
// IProjectService.cs - The contract
public interface IProjectService
{
    Task<IReadOnlyList<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(int id);
    Task<Project> CreateAsync(Project project);
    Task<Project?> UpdateAsync(Project project);
}

// ProjectService.cs - The implementation
public class ProjectService : IProjectService
{
    public async Task<IReadOnlyList<Project>> GetAllAsync() { ... }
    public async Task<Project?> GetByIdAsync(int id) { ... }
    public async Task<Project> CreateAsync(Project project) { ... }
    public async Task<Project?> UpdateAsync(Project project) { ... }
}
```

### 5. The Blazor Rendering Cycle

When you modify data and want the page to update:

```csharp
// 1. Modify data
_title = "New Title";

// 2. Tell Blazor to re-render
StateHasChanged();

// 3. Blazor re-runs the @code block and re-renders HTML
// 4. User sees updated content
```

---

## Flow Summary

Every user action follows this flow:

```
┌─────────────────────────────────────────────────────────┐
│ 1. USER INTERACTION (UI Layer)                          │
│    └─ User clicks button or fills form                  │
├─────────────────────────────────────────────────────────┤
│ 2. EVENT HANDLER (UI Layer)                             │
│    └─ HandleClick() or HandleSubmit()                   │
├─────────────────────────────────────────────────────────┤
│ 3. SERVICE CALL (Service Layer)                         │
│    └─ await ProjectService.CreateAsync(project)         │
├─────────────────────────────────────────────────────────┤
│ 4. DATABASE OPERATION (Data Layer)                      │
│    └─ _dbContext.Projects.Add(project)                  │
│    └─ await _dbContext.SaveChangesAsync()               │
├─────────────────────────────────────────────────────────┤
│ 5. RESPONSE (Service → UI)                              │
│    └─ Return result to UI layer                         │
├─────────────────────────────────────────────────────────┤
│ 6. UPDATE DISPLAY (UI Layer)                            │
│    └─ StateHasChanged()                                 │
│    └─ Navigate to different page                        │
├─────────────────────────────────────────────────────────┤
│ 7. USER SEES RESULT                                     │
│    └─ New project appears in list                       │
└─────────────────────────────────────────────────────────┘
```

---

## Summary

The Academic Project Manager follows a clean, organized architecture:

- **Data Layer** stores what (entities, database)
- **Service Layer** implements how (business logic, calculations)
- **UI Layer** shows what and lets users do things (Blazor components)

Each layer has a single responsibility, making the code easy to understand, test, and maintain. Users interact with the UI, which talks to services, which talk to the database. Simple, clean, effective.
