#!/usr/bin/env python3
import subprocess
import sys

# Install reportlab
subprocess.check_call([sys.executable, "-m", "pip", "install", "reportlab", "-q"])

from reportlab.lib.pagesizes import letter
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.units import inch
from reportlab.platypus import SimpleDocTemplate, Paragraph, Spacer, Table, TableStyle, PageBreak
from reportlab.lib import colors
from reportlab.lib.enums import TA_CENTER, TA_LEFT, TA_JUSTIFY
from datetime import datetime

# Create PDF
pdf_path = 'Academic_Project_Manager_Documentation.pdf'
doc = SimpleDocTemplate(pdf_path, pagesize=letter, topMargin=0.75*inch, bottomMargin=0.75*inch)

# Get sample styles
styles = getSampleStyleSheet()

# Custom styles
title_style = ParagraphStyle(
    'CustomTitle',
    parent=styles['Heading1'],
    fontSize=24,
    textColor=colors.HexColor('#1e40af'),
    spaceAfter=6,
    alignment=TA_CENTER,
    fontName='Helvetica-Bold'
)

heading_style = ParagraphStyle(
    'CustomHeading',
    parent=styles['Heading2'],
    fontSize=14,
    textColor=colors.HexColor('#1e40af'),
    spaceAfter=6,
    spaceBefore=12,
    fontName='Helvetica-Bold'
)

subheading_style = ParagraphStyle(
    'CustomSubheading',
    parent=styles['Heading3'],
    fontSize=11,
    textColor=colors.HexColor('#3b82f6'),
    spaceAfter=4,
    fontName='Helvetica-Bold'
)

body_style = ParagraphStyle(
    'CustomBody',
    parent=styles['BodyText'],
    fontSize=10,
    alignment=TA_JUSTIFY,
    spaceAfter=8,
    leading=14
)

# Build document
elements = []

# Title Page
elements.append(Spacer(1, 1.5*inch))
elements.append(Paragraph('Academic Project Manager', title_style))
elements.append(Spacer(1, 0.3*inch))
elements.append(Paragraph('3-Layer Architecture with Real-Time Analytics', ParagraphStyle(
    'Subtitle',
    parent=styles['Normal'],
    fontSize=13,
    textColor=colors.HexColor('#4b5563'),
    alignment=TA_CENTER,
    spaceAfter=12
)))
elements.append(Spacer(1, 0.5*inch))

dev_table = Table([
    ['Developed By:'],
    ['Ahmed Ameur LEJMI'],
    ['Abdelmajid TABESSI'],
    ['Houssem Eddine BOUZAMOUCHA']
], colWidths=[6*inch])
dev_table.setStyle(TableStyle([
    ('ALIGN', (0, 0), (-1, -1), 'CENTER'),
    ('FONTNAME', (0, 0), (-1, 0), 'Helvetica-Bold'),
    ('FONTSIZE', (0, 0), (-1, -1), 10),
    ('BOTTOMPADDING', (0, 0), (-1, -1), 6),
]))
elements.append(dev_table)

elements.append(Spacer(1, 1*inch))
elements.append(Paragraph(f'Date: {datetime.now().strftime("%B %d, %Y")}', ParagraphStyle(
    'DateStyle',
    parent=styles['Normal'],
    fontSize=10,
    alignment=TA_CENTER
)))

elements.append(PageBreak())

# Table of Contents
elements.append(Paragraph('Table of Contents', heading_style))
elements.append(Spacer(1, 0.2*inch))

toc_items = [
    '1. Executive Summary',
    '2. System Overview',
    '3. Technology Stack',
    '4. Architecture Design',
    '5. Core Features',
    '6. Database Design',
    '7. Service Layer',
    '8. Analytics & Risk Management'
]

for item in toc_items:
    elements.append(Paragraph(item, body_style))

elements.append(PageBreak())

# 1. Executive Summary
elements.append(Paragraph('1. Executive Summary', heading_style))
elements.append(Spacer(1, 0.1*inch))

elements.append(Paragraph(
    'The Academic Project Manager is a comprehensive web-based application designed to streamline project management, task assignment, and team collaboration in academic and professional environments. Built with modern technologies including .NET 10.0, Blazor Web App, and Entity Framework Core, the application provides real-time project analytics, intelligent risk prediction, and intuitive task management capabilities.',
    body_style
))

elements.append(Paragraph(
    'The system employs a strict three-layer architecture separating data persistence, business logic, and user interface concerns. This design ensures scalability, maintainability, and clear separation of responsibilities. All database operations utilize SQLite for portability and zero-configuration deployment, while comprehensive seed data enables immediate productivity upon deployment.',
    body_style
))

elements.append(Paragraph(
    'Key objectives include enabling project managers to track project health, identify at-risk tasks through predictive analytics, monitor team workload balance, and make data-driven decisions about task assignments and project timelines.',
    body_style
))

elements.append(PageBreak())

# 2. System Overview
elements.append(Paragraph('2. System Overview', heading_style))
elements.append(Spacer(1, 0.1*inch))

elements.append(Paragraph('2.1 Purpose and Scope', subheading_style))
elements.append(Paragraph(
    'Academic Project Manager provides a centralized platform for managing multiple concurrent projects with distributed team members. The system tracks project progress, monitors individual task completion rates, identifies scheduling conflicts, and predicts potential delays through intelligent risk analysis algorithms.',
    body_style
))

elements.append(Paragraph('2.2 Key Stakeholders', subheading_style))
elements.append(Paragraph(
    'Primary users include project managers who oversee multiple projects, team members who execute assigned tasks, and team leads responsible for resource allocation and workload balancing. The system serves both individual contributors and management-level decision makers.',
    body_style
))

elements.append(Paragraph('2.3 Primary Use Cases', subheading_style))

use_cases = [
    'Create and manage multiple projects with detailed descriptions and deadlines',
    'Register team members with roles and contact information',
    'Assign specific tasks to team members with deadline tracking',
    'Monitor real-time project health scores and completion percentages',
    'Track individual and team completion rates through interactive dashboards',
    'Identify high-risk tasks through predictive delay analysis',
    'Update task status through an intuitive workflow system',
    'Generate historical performance metrics and team utilization reports'
]

for uc in use_cases:
    elements.append(Paragraph(f'• {uc}', body_style))

elements.append(PageBreak())

# 3. Technology Stack
elements.append(Paragraph('3. Technology Stack', heading_style))
elements.append(Spacer(1, 0.1*inch))

elements.append(Paragraph('3.1 Backend Technologies', subheading_style))

tech_data = [
    ['.NET 10.0', 'Modern, high-performance framework for building web applications'],
    ['Blazor Web App', 'Interactive server-side rendering with real-time event handling'],
    ['Entity Framework Core 10.0.8', 'Object-Relational Mapping for database abstraction'],
    ['SQLite', 'File-based relational database for zero-configuration deployment'],
    ['Async/Await', 'Non-blocking operations for responsive user experience']
]

tech_table = Table(tech_data, colWidths=[1.8*inch, 4.2*inch])
tech_table.setStyle(TableStyle([
    ('BACKGROUND', (0, 0), (-1, 0), colors.HexColor('#1e40af')),
    ('TEXTCOLOR', (0, 0), (-1, 0), colors.whitesmoke),
    ('ALIGN', (0, 0), (-1, -1), 'LEFT'),
    ('FONTNAME', (0, 0), (-1, 0), 'Helvetica-Bold'),
    ('FONTSIZE', (0, 0), (-1, -1), 9),
    ('BOTTOMPADDING', (0, 0), (-1, 0), 8),
    ('GRID', (0, 0), (-1, -1), 1, colors.grey),
    ('ROWBACKGROUNDS', (0, 1), (-1, -1), [colors.white, colors.HexColor('#f3f4f6')])
]))
elements.append(tech_table)

elements.append(Spacer(1, 0.2*inch))
elements.append(Paragraph('3.2 Frontend Technologies', subheading_style))

elements.append(Paragraph(
    'The user interface utilizes HTML5, CSS3 with Bootstrap 5 framework for responsive design, and C# Razor components for server-side rendering. All interactive components are rendered with @rendermode InteractiveServer to enable real-time event handling and dynamic updates without page refresh.',
    body_style
))

elements.append(Spacer(1, 0.15*inch))
elements.append(Paragraph('3.3 Architecture Pattern', subheading_style))

elements.append(Paragraph(
    'The application implements the Repository and Service Pattern, ensuring clean separation between data access, business logic, and presentation layers. Dependency Injection manages component lifecycle and enables testability through interface-based abstraction.',
    body_style
))

elements.append(PageBreak())

# 4. Architecture Design
elements.append(Paragraph('4. Architecture Design', heading_style))
elements.append(Spacer(1, 0.1*inch))

elements.append(Paragraph('4.1 Three-Layer Architecture', subheading_style))

elements.append(Paragraph(
    'The system strictly separates concerns into three distinct layers:',
    body_style
))

elements.append(Spacer(1, 0.1*inch))

elements.append(Paragraph('Data Layer (AcademicProjectManager.Data)', subheading_style))
elements.append(Paragraph(
    'Manages all data persistence using Entity Framework Core with SQLite. Contains database context, entity models with Data Annotations validation, migration scripts, and seed data generators. All queries utilize LINQ with eager loading via Include() for optimal performance.',
    body_style
))

elements.append(Spacer(1, 0.15*inch))

elements.append(Paragraph('Service Layer (AcademicProjectManager.Services)', subheading_style))
elements.append(Paragraph(
    'Implements business logic through specialized service classes: ProjectService, MemberService, and TaskService. Each service provides async Task<T> methods for CRUD operations and specialized business logic including health score calculation, performance metrics, and predictive risk analysis.',
    body_style
))

elements.append(Spacer(1, 0.15*inch))

elements.append(Paragraph('Presentation Layer (AcademicProjectManager.UI)', subheading_style))
elements.append(Paragraph(
    'Displays data and captures user input through Blazor components organized by feature: Home dashboard, Projects list and detail pages, Members management, and ProjectDetails with inline task board. All forms utilize explicit event handlers for reliable data binding.',
    body_style
))

elements.append(PageBreak())

# 5. Core Features
elements.append(Paragraph('5. Core Features', heading_style))
elements.append(Spacer(1, 0.1*inch))

elements.append(Paragraph('5.1 Project Management', subheading_style))
elements.append(Paragraph(
    'Users can create projects with title, description, start date, and deadline. Each project tracks completion percentage, overdue task count, and health score. Projects display all assigned tasks in an inline board with status management.',
    body_style
))

elements.append(Paragraph('5.2 Member Management', subheading_style))
elements.append(Paragraph(
    'Team members are registered with first name, last name, email, and role information. The system tracks each member\'s total assigned tasks, completion rate, workload, and historical delay patterns for predictive analysis.',
    body_style
))

elements.append(Paragraph('5.3 Task Management', subheading_style))
elements.append(Paragraph(
    'Tasks are created with detailed descriptions, assigned deadlines, and team member assignments. Tasks flow through workflow states: Pending → InProgress → Completed. The system automatically tracks CompletedOn timestamp when tasks reach Completed status.',
    body_style
))

elements.append(Spacer(1, 0.15*inch))

elements.append(Paragraph('5.4 Real-Time Analytics Dashboard', subheading_style))
elements.append(Paragraph(
    'The Home dashboard displays Key Performance Indicators including active project count, delayed task count, and average project health score. Project Health Overview shows each project\'s completion percentage and overdue task count. Member Completion Rate tracks individual and team performance with visual progress bars and color-coded risk levels.',
    body_style
))

elements.append(PageBreak())

# 6. Database Design
elements.append(Paragraph('6. Database Design', heading_style))
elements.append(Spacer(1, 0.1*inch))

elements.append(Paragraph('6.1 Entity Relationships', subheading_style))

elements.append(Paragraph(
    'The database consists of three primary entities with carefully designed relationships:',
    body_style
))

elements.append(Spacer(1, 0.1*inch))

elements.append(Paragraph('Member Entity', subheading_style))
elements.append(Paragraph(
    'Stores team member information including FirstName, LastName, Email, and Role. Relationships: One-to-Many with ProjectTasks (assigned tasks). OnDelete: Restrict ensures data integrity when deleting members with assigned tasks.',
    body_style
))

elements.append(Spacer(1, 0.1*inch))

elements.append(Paragraph('Project Entity', subheading_style))
elements.append(Paragraph(
    'Manages project metadata including Title, Description, StartDate, Deadline, and IsCompleted status. Relationships: One-to-Many with ProjectTasks. OnDelete: Cascade automatically removes all project tasks when a project is deleted.',
    body_style
))

elements.append(Spacer(1, 0.1*inch))

elements.append(Paragraph('ProjectTask Entity', subheading_style))
elements.append(Paragraph(
    'Represents individual work items with Title, Description, Deadline, Status (enum), and AssignedMemberId. Includes tracking fields: AssignedOn, CompletedOn, and IsOverdue. Foreign keys enforce referential integrity to Member and Project entities.',
    body_style
))

elements.append(Spacer(1, 0.15*inch))

elements.append(Paragraph('6.2 Seed Data', subheading_style))
elements.append(Paragraph(
    'The database initializes with comprehensive sample data: 5 team members across various roles, 3 projects with realistic timelines, and 12 tasks distributed across projects with mixed completion states. This enables immediate testing and demonstration without manual data entry.',
    body_style
))

elements.append(PageBreak())

# 7. Service Layer
elements.append(Paragraph('7. Service Layer', heading_style))
elements.append(Spacer(1, 0.1*inch))

elements.append(Paragraph('7.1 ProjectService', subheading_style))
elements.append(Paragraph(
    'Provides CRUD operations for projects and calculates project health scores. Health Score = Completion Rate - (Overdue Tasks × 12%), clamped to 0-100 range. Queries include eager-loaded tasks for optimal performance.',
    body_style
))

elements.append(Paragraph('7.2 MemberService', subheading_style))
elements.append(Paragraph(
    'Manages team member CRUD operations and calculates performance metrics. Completion Rate = (Completed Tasks / Total Assigned Tasks) × 100. Returns MemberPerformanceResult objects containing completion rates, workload analysis, and utilization metrics.',
    body_style
))

elements.append(Paragraph('7.3 TaskService', subheading_style))
elements.append(Paragraph(
    'Handles task CRUD operations, status workflow management, and delay calculations. CalculateDelaysAsync() marks tasks as overdue when deadline < current time. UpdateTaskStatusAsync() sets CompletedOn timestamp when transitioning to Completed status.',
    body_style
))

elements.append(Spacer(1, 0.15*inch))

elements.append(Paragraph('7.4 Predictive Risk Analysis', subheading_style))
elements.append(Paragraph(
    'GetPredictiveRisksAsync() analyzes each pending/in-progress task to calculate risk factor based on: (Utilization Rate × 0.4) + (Delay Behavior Score × 0.35) + (Time Pressure Score × 0.25). Risk levels classify as High (≥70), Medium (45-69), or Low (<45) to flag tasks requiring attention.',
    body_style
))

elements.append(PageBreak())

# 8. Analytics and Risk Management
elements.append(Paragraph('8. Analytics & Risk Management', heading_style))
elements.append(Spacer(1, 0.1*inch))

elements.append(Paragraph('8.1 Key Performance Indicators', subheading_style))

kpis = [
    ('Active Projects', 'Count of projects with incomplete tasks or health score < 100'),
    ('Delayed Tasks', 'Sum of overdue tasks across all projects'),
    ('Average Health Score', 'Mean health score across all projects, indicating overall portfolio health')
]

for kpi_name, kpi_desc in kpis:
    elements.append(Paragraph(f'• <b>{kpi_name}:</b> {kpi_desc}', body_style))

elements.append(Spacer(1, 0.15*inch))

elements.append(Paragraph('8.2 Member Performance Metrics', subheading_style))
elements.append(Paragraph(
    'The Workload Balance section displays each member\'s active task count and completion percentage. Members are sorted by workload to identify over-allocation. Completion rates use color-coded progress bars: Green (≥75%), Blue (50-74%), Yellow (25-49%), Red (<25%).',
    body_style
))

elements.append(Spacer(1, 0.15*inch))

elements.append(Paragraph('8.3 Predictive Delay Warning System', subheading_style))
elements.append(Paragraph(
    'The dashboard displays the top 5 highest-risk pending tasks. Each task shows member assignment, project context, risk factor percentage, and severity level. High-risk tasks generate recommendations to reassign work before deadline violations occur.',
    body_style
))

elements.append(Spacer(1, 0.15*inch))

elements.append(Paragraph('8.4 Data-Driven Decision Support', subheading_style))
elements.append(Paragraph(
    'Project managers use analytics to identify over-allocated team members, predict delays, and redistribute work proactively. Historical completion rates inform realistic deadline setting, while health scores provide portfolio-level visibility into project status.',
    body_style
))

# Build PDF
doc.build(elements)
print(f'PDF created successfully: {pdf_path}')
