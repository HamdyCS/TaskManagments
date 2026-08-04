
# TaskManagments

A team-oriented project and task management API built with ASP.NET Core, following Clean Architecture and CQRS principles.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-13-239120?style=flat-square&logo=csharp)](https://learn.microsoft.com/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](LICENSE)

[Overview](#overview) • [Features](#features) • [Architecture](#architecture) • [Getting Started](#getting-started) • [API Reference](#api-reference) • [Configuration](#configuration)

## Overview

TaskManagments is a RESTful API for managing workspaces, projects, and tasks with team collaboration. It supports role-based access control (Owner, Project Manager, Member), real-time notifications via SignalR, OAuth authentication, and generates PDF reports.

> [!NOTE]
> This is a backend-only API project. A frontend client (e.g. Angular) is expected to consume this API.

## Features

- **Workspaces & Projects** -- Organize work into workspaces containing multiple projects
- **Task Management** -- Create, assign, track, and comment on tasks with priorities and deadlines
- **Role-Based Access** -- Workspace Owner, Project Manager, and Member roles with fine-grained permissions
- **Authentication** -- JWT (cookie-based) with Google OAuth support, OTP for sensitive operations
- **Real-time Notifications** -- SignalR hub for live workspace notifications (task assignments, comments, invites)
- **File Attachments** -- Upload and manage task attachments (up to 50 MB)
- **Reporting** -- Workspace reports by task status/priority, member performance, and PDF export
- **Workspace Invites** -- Invite users to workspaces with role assignment and expiry

## Architecture

```
Api (Presentation)  -->  Application  -->  Domain  <--  Infrastructure
```

| Layer | Responsibility |
|-------|---------------|
| **Api** | Controllers, SignalR hubs, auth policies, exception handling |
| **Application** | CQRS features (commands/queries), validators, DTOs, interfaces |
| **Domain** | Entities, enums, pagination, domain interfaces (no dependencies) |
| **Infrastructure** | EF Core + SQL Server, Redis cache, Identity, MailKit, background services |

Key patterns: **CQRS** (MediatR), **Repository + Unit of Work**, **ErrorOr** result pattern, **FluentValidation**, **Mapster** mapping, **Soft Delete**.

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (LocalDB or full instance)
- [Redis](https://redis.io/) (for caching)

### Clone and run

```bash
git clone https://github.com/<your-username>/TaskManagments.git
cd TaskManagments
dotnet restore
dotnet build
dotnet run --project src/Api/Api.csproj
```

The API starts at `http://localhost:5102` by default.

### Apply database migrations

```bash
dotnet ef database update \
  --project src/Infrastructure/Infrastructure.csproj \
  --startup-project src/Api/Api.csproj
```

> [!TIP]
> Connection strings are configured in `appsettings.Development.json`. The default uses `Server=.;Database=TaskManagementsDB;Integrated Security=True`.

## API Reference

### Authentication

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register-user` | Register a new user |
| POST | `/api/auth/login` | Login (returns JWT in cookies) |
| POST | `/api/auth/logout` | Logout |
| GET | `/api/auth/login-user-with-google` | Google OAuth login |
| POST | `/api/auth/refresh-token` | Refresh access token |

### Workspaces

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/workspaces` | Create workspace |
| GET | `/api/workspaces/{id}` | Get workspace by ID |
| PUT | `/api/workspaces/{id}` | Update workspace (Owner) |
| DELETE | `/api/workspaces/{id}` | Delete workspace (Owner) |

### Projects

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/workspaces/{workspaceId}/projects` | Create project |
| GET | `/api/workspaces/{workspaceId}/projects` | List all projects |
| GET | `/api/workspaces/{workspaceId}/projects/{projectId}` | Get project |
| PUT | `/api/workspaces/{workspaceId}/projects/{projectId}` | Update project |
| PATCH | `/api/workspaces/{workspaceId}/projects/{projectId}/status` | Update project status |
| DELETE | `/api/workspaces/{workspaceId}/projects/{projectId}` | Delete project |

### Tasks

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `.../projects/{projectId}/tasks` | Create task |
| GET | `.../projects/{projectId}/tasks` | List all project tasks |
| GET | `.../tasks/{taskId}` | Get task by ID |
| PUT | `.../tasks/{taskId}` | Update task |
| DELETE | `.../tasks/{taskId}` | Delete task |
| POST | `.../tasks/{taskId}/assignments` | Assign user to task |
| PATCH | `.../tasks/{taskId}/status` | Change task status |

### Comments & Attachments

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `.../tasks/{taskId}/comments` | Add comment |
| GET | `.../tasks/{taskId}/comments` | List comments |
| POST | `.../tasks/{taskId}/attachments` | Upload attachment |
| GET | `.../tasks/{taskId}/attachments` | List attachments |

### Reports

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/workspaces/{id}/reports` | Workspace overview report |
| GET | `.../reports/projects/{id}/tasks-by-status` | Tasks by status |
| GET | `.../reports/projects/{id}/tasks-by-priority` | Tasks by priority |
| GET | `.../reports/members/{id}/performance` | Member performance |
| GET | `.../reports/pdf` | PDF report download |

### Notifications (SignalR)

Connect to `/notificationHub` and call `JoinWorkSpace(workSpaceId)` to receive real-time notifications for a workspace.

## Configuration

All configuration is in `appsettings.json` / `appsettings.Development.json`.

| Section | Description |
|---------|-------------|
| `ConnectionStrings:SqlServer` | SQL Server connection string |
| `ConnectionStrings:Redis` | Redis connection string |
| `Jwt` | JWT signing key, issuer, audience, lifetime |
| `RefreshToken` | Refresh token lifetime in days |
| `Otp` | OTP lifetime in minutes |
| `Mail` | SMTP email configuration |
| `WorkSpaceInvite` | Invite expiry in days |
| `Authentication:Google` | Google OAuth client ID/secret |

> [!IMPORTANT]
> JWT tokens are stored in **HttpOnly cookies** (`access_token` and `refresh_token`), not in the `Authorization` header.

## Tech Stack

| Technology | Purpose |
|------------|---------|
| ASP.NET Core 10 | Web framework |
| Entity Framework Core | ORM + migrations |
| SQL Server | Primary database |
| Redis | Distributed caching |
| ASP.NET Identity | User management |
| MediatR | CQRS dispatcher |
| FluentValidation | Request validation |
| Mapster | Object mapping |
| ErrorOr | Functional error handling |
| SignalR | Real-time notifications |
| MailKit | Email sending |
| QuestPDF | PDF report generation |
| Serilog + Seq | Structured logging |
