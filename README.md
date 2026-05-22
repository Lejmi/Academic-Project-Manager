# Academic Project Manager

Academic Project Manager is a Blazor web application that helps teams manage projects, tasks, and team member performance. It uses a 3-layer architecture (Data, Services, UI), Entity Framework Core with SQLite, and real-time interactive Blazor components.

## Quick Start

1. Install .NET 10 SDK: https://dotnet.microsoft.com
2. Restore and build:

```powershell
dotnet build .\AcademicProjectManager.slnx
```

3. Run the UI project:

```powershell
dotnet run --project .\AcademicProjectManager.UI\AcademicProjectManager.UI.csproj
```

4. Open http://localhost:5146 in your browser.

## Notes

- The repository uses SQLite for easy local development. The database file is stored locally and seeded with example data.
- The `Academic_Project_Manager_Documentation.pdf` file is intentionally excluded from the repository via `.gitignore`.

## Developers

- Ahmed Ameur LEJMI
- Abdelmajid TABESSI
- Houssem Eddine BOUZAMOUCHA

