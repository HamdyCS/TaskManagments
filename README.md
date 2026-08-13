
# TaskManagments

A team-oriented **project and task management API** built with ASP.NET Core, following **Clean Architecture** and **CQRS** principles. Manage workspaces, projects, and tasks with fine-grained role-based permissions, real-time notifications via SignalR, OAuth authentication, and PDF reporting.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-13-239120?style=flat-square&logo=csharp)](https://learn.microsoft.com/dotnet/csharp/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-10-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](LICENSE)

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Domain Model](#domain-model)
- [Authentication & Authorization](#authentication--authorization)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [API Reference](#api-reference)
- [Pagination](#pagination)
- [Real-time Notifications (SignalR)](#real-time-notifications-signalr)
- [Error Handling](#error-handling)
- [Project Structure](#project-structure)

---

## Overview

TaskManagments is a **RESTful, backend-only API** for organizing work into workspaces that contain projects and tasks, with team collaboration features:

- Role-based access control across workspace members (**Owner**, **Project Manager**, **Member**) plus a global **Admin** role.
- Task assignment, commenting, and file attachments.
- Real-time push notifications through a SignalR hub.
- Workspace invitations with role assignment and expiry.
- Analytical reports (tasks by status/priority, member performance) and **PDF** export.
- Full authentication lifecycle: registration, email confirmation, login (JWT via cookies), Google OAuth, OTP verification, password reset, email change, and account deletion.

> [!NOTE]
> This project is an API only. A frontend client (e.g. Angular) is expected to consume it. The default base URL is `http://localhost:5102`.

---

## Features

- **Workspaces & Projects** — Organize work into workspaces that contain multiple projects.
- **Task Management** — Create, assign, track, and comment on tasks with priorities, statuses, and deadlines.
- **Role-Based Access** — Workspace `Owner`, `ProjectManager`, and `Member` roles with granular, per-request authorization.
- **Authentication** — JWT stored in HttpOnly cookies, Google OAuth, OTP for sensitive operations, email confirmation, password reset, and email change flows.
- **Real-Time Notifications** — SignalR hub for live workspace notifications (task assigned, status changed, comments, invites).
- **File Attachments** — Upload/download task attachments (`.pdf`, `.jpg`, `.jpeg`, `.png`, up to **50 MB**).
- **Reporting** — Workspace overview report, tasks by status/priority, member performance, and a PDF report download.
- **Workspace Invites** — Invite users to a workspace with a role and expiry, accept/reject from the invitee side.

---

## Architecture

Clean Architecture with strict dependency rules:

```
Api (Presentation)  -->  Application  -->  Domain  <--  Infrastructure
```

| Layer | Responsibility |
|-------|---------------|
| **Api** | Controllers, SignalR hubs, auth policies & handlers, exception handling, CORS/origin checks |
| **Application** | CQRS features (commands/queries), FluentValidation validators, DTOs, mapping, service/repository interfaces, errors |
| **Domain** | Entities, enums, pagination model, `ISoftDelete` / `IBaseEntity` interfaces (zero dependencies) |
| **Infrastructure** | EF Core + SQL Server, Redis cache, ASP.NET Identity, MailKit email, background services, repositories |

Key patterns:

- **CQRS** with MediatR — every feature is a command/query handled by a dedicated handler.
- **Repository + Unit of Work** — all data access flows through `IUnitOfWork`; repositories are never injected into handlers directly.
- **ErrorOr** result pattern — handlers return `ErrorOr<T>` for expected failures instead of throwing.
- **FluentValidation** — request validation on every command/query.
- **Mapster** — object mapping (via `.Adapt<T>()` and `IRegister` mapping classes).
- **Soft Delete** — entities implementing `ISoftDelete` are soft-deleted by the generic repository.

---

## Tech Stack

| Technology | Purpose |
|------------|---------|
| ASP.NET Core 10 | Web framework (controllers, minimal APIs, middleware) |
| Entity Framework Core | ORM + migrations (SQL Server) |
| SQL Server | Primary database |
| Redis | Distributed caching (reports, etc.) |
| ASP.NET Core Identity | User management (users extend `IdentityUser`) |
| MediatR | CQRS command/query dispatcher |
| FluentValidation | Request validation |
| Mapster | Object mapping |
| ErrorOr | Functional error handling |
| SignalR | Real-time notifications (`/notificationHub`) |
| MailKit | SMTP email (confirmation, OTP, reset, invites) |
| QuestPDF | PDF report generation |
| Serilog + Seq | Structured logging |

---

## Domain Model

All entities live in `src/Domain/Entities/`.

| Entity | Notes |
|--------|-------|
| `User` | Extends `IdentityUser`. **String** primary key. Has `FirstName`, `LastName`, `DateOfBirth`. |
| `WorkSpace` | Top-level container (`long Id`), tracks `CreatedById`, `LastUpdatedById`. Soft-deletable. |
| `WorkSpaceUser` | Many-to-many `User` ↔ `WorkSpace` join with a `WorkSpaceRole`. |
| `Project` | Belongs to a `WorkSpace`, has a `ProjectStatus`. Soft-deletable. |
| `ProjectTask` | Belongs to a `Project`, has `TaskStatus`, `TaskPriority`, `Deadline`. Soft-deletable. |
| `TaskAssignment` | Assigns a user to a task (`AssignedToId`), tracks who assigned and when it was unassigned. |
| `TaskComment` | Comments on a task by a user. |
| `TaskAttachment` | File metadata on a task. |
| `Notification` | Per-user notification (optionally linked to a task or workspace invite). |
| `RefreshToken` | Refresh tokens for the JWT rotation flow. |
| `WorkSpaceInvite` | Invitation to join a workspace with role and expiry. |

### Enums

All enums are serialized as **strings** in JSON (`JsonStringEnumConverter`).

| Enum | Values |
|------|--------|
| `Role` | `Admin`, `User` |
| `WorkSpaceRole` | `Owner`, `ProjectManager`, `Member` |
| `ProjectStatus` | `Active`, `OnHold`, `Completed` |
| `ProjectTaskStatus` | `Backlog`, `Todo`, `InProgress`, `Review`, `Done` |
| `TaskPriority` | `Low`, `Medium`, `High`, `Critical` |
| `WorkSpaceInviteStatus` | `Pending`, `Accepted`, `Rejected` |
| `NotificationType` | `TaskAssigned`, `TaskUnassigned`, `TaskStatusUpdated`, `TaskUpdated`, `CommentAdded`, `DueDateReminder`, `TaskDeleted`, `WorkSpaceInvite` |
| `OtpPurpose` | `ForgetPassword`, `DeleteAccount` |
| `Provider` | `Google` |

> [!TIP]
> Because IDs are passed as route params and enums as strings, JSON bodies use `"status": "InProgress"` rather than numbers.

---

## Authentication & Authorization

### JWT via cookies

- JWT access tokens are read from the **`access_token` HttpOnly cookie**, not the `Authorization` header.
- Refresh tokens are stored in the **`refresh_token`** cookie.
- Login/refresh/logout endpoints set or clear these cookies automatically.
- All protected endpoints expect the cookie to be sent by the browser (or an HTTP client configured to send cookies).

### Roles & policies

| Role / Policy | Meaning |
|---------------|---------|
| `Admin` (global role) | Bypasses workspace membership checks. Can manage users, list all workspaces, etc. |
| `WorkSpaceOwner` | The user is the **Owner** of the workspace. |
| `WorkSpaceUser` | The user is a member of the workspace (any role). |
| `WorkSpaceProjectManager` | The user is a **ProjectManager** in the workspace. |

Controllers perform inline authorization via `IAuthorizationService.AuthorizeAsync(User, resourceId, policyName)`. Handlers for these policies live in `src/Api/Polices/WorkSpace/`.

### Google OAuth

- `GET /api/auth/login-user-with-google?returnUrl=...` triggers the OAuth challenge.
- The callback `GET /api/auth/login-user-by-provider-callback?returnUrl=...` completes login and redirects back.
- `returnUrl` must be an allowed origin (see `Api/Common/Origins/AllowOrigin.cs`).

### Registration rules

- First/last name: 2–50 characters.
- Email: valid email address.
- Password: 8–80 characters, must contain at least one lowercase, one uppercase, one digit, and one special character (`!@#$%^&*()_+=-`).
- Date of birth: must be at least **18 years old**.
- New accounts must **confirm their email** before logging in (`POST /api/auth/confirm-email`).

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (LocalDB or a full instance)
- [Redis](https://redis.io/) (caching)
- Optional: [Seq](http://localhost:5341) for structured log ingestion

### Clone & run

```bash
git clone https://github.com/<your-username>/TaskManagments.git
cd TaskManagments
dotnet restore
dotnet build
dotnet run --project src/Api/Api.csproj
```

The API starts at `http://localhost:5102` (see `src/Api/Properties/launchSettings.json`).

### Apply database migrations

```bash
dotnet ef database update \
  --project src/Infrastructure/Infrastructure.csproj \
  --startup-project src/Api/Api.csproj
```

To create a new migration:

```bash
dotnet ef migrations add <Name> \
  --project src/Infrastructure/Infrastructure.csproj \
  --startup-project src/Api/Api.csproj
```

> [!TIP]
> Connection strings live in `appsettings.Development.json` under the `SqlServer` key (not `DefaultConnection`). Redis is under the `Redis` key.

---

## Configuration

All configuration lives in `appsettings.json` / `appsettings.Development.json`.

| Section | Description | Default (dev) |
|---------|-------------|---------------|
| `ConnectionStrings:SqlServer` | SQL Server connection string | `Server=.;Database=TaskManagementsDB;Integrated Security=True;...` |
| `ConnectionStrings:Redis` | Redis connection string | `localhost:6379` |
| `Jwt:SigningKey` | JWT signing key (from user-secrets / env in production) | — |
| `Jwt:Issuer` | Token issuer | `https://localhost:7018` |
| `Jwt:Audience` | Token audience | `http://localhost:4200` |
| `Jwt:LifeTimeMinutes` | Access-token lifetime | `20` |
| `RefreshToken:LifeTimeDays` | Refresh-token lifetime | `30` |
| `Otp:LifeTimeInMinutes` | OTP lifetime | `60` |
| `WorkSpaceInvite:LifeTimeDays` | Invite expiry | `60` |
| `Mail` | SMTP settings (Email, AppPassword, Host, Port) | Ethereal test inbox |
| `settings:frontendUrl` | Frontend origin used in emails/redirects | `http://localhost:4200` |
| `Serilog` | Serilog sinks (Console + Seq) and levels | Seq at `localhost:5341` |
| `Authentication:Google` | Google OAuth ClientId / ClientSecret | — |

> [!IMPORTANT]
> Secrets such as `Jwt:SigningKey` and `Authentication:Google` must **not** be committed. Provide them via environment variables or user-secrets in production.

---

## API Reference

> Conventions used below:
> - Base URL: `http://localhost:5102`
> - Authentication is via the `access_token` cookie (set by login). Endpoints marked **🔒** require an authenticated user.
> - Route values: `{workspaceId}` / `{workSpaceId}` and `{projectId}` are `long`; `{userId}` / `{memberId}` are `string` (Identity IDs); `{taskId}`, `{commentId}`, `{attachmentId}`, `{id}` are `long`.
> - Pagination endpoints accept `?pageNumber=1&pageSize=10` (see [Pagination](#pagination)).
> - All errors are returned as **RFC 7807 Problem Details** (`errors.ToProblemDetailsObjectResult()`).

---

### Endpoint index (grouped by controller)

| # | Controller | Base route | APIs |
|---|------------|------------|------|
| 1 | `AuthController` | `/api/auth` | `POST register-user` · `POST register-admin` · `POST confirm-email` · `POST login` · `POST refresh-token` · `POST logout` · `GET ""` · `PUT ""` · `POST forget-password/send-otp` · `POST forget-password/resend-otp` · `POST forget-password` · `POST reset-password/send-email` · `POST reset-password` · `POST change-email/send-email` · `POST change-email` · `POST delete-account/send-otp` · `POST delete-account/resend-otp` · `DELETE delete-account` · `GET login-user-with-google` · `GET login-user-by-provider-callback` |
| 2 | `UsersController` | `/api/users` | `GET {id}` · `GET all` · `DELETE {id}` |
| 3 | `WorkSpacesController` | `/api/workspaces` | `GET {id}` · `GET all` · `GET {id}/all-users` · `POST ""` · `PUT {id}` · `DELETE {id}` |
| 4 | `WorkSpaceInvitesController` | `/api/workspace-invites` | `GET {id}` · `GET all-my-invites` · `GET all-my-send-invites` · `POST ""` · `DELETE {id}` · `PATCH {id}/accept` · `PATCH {id}/reject` |
| 5 | `ProjectsController` | `/api/workspaces/{workspaceId}/projects` | `POST ""` · `GET {projectId}` · `GET ""` · `PUT {projectId}` · `PATCH {projectId}/status` · `DELETE {projectId}` |
| 6 | `ProjectsTasksController` | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks` | `POST ""` · `GET {taskId}` · `GET {taskId}/me` · `GET ""` · `GET users/{userId}` · `GET me` · `PUT {taskId}` · `DELETE {taskId}` · `POST {taskId}/assignments` · `DELETE {taskId}/assignments/{assignedUserId}` · `PATCH {taskId}/status` · `PATCH {taskId}/me/status` |
| 7 | `TaskCommentsController` | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/comments` | `POST ""` · `GET ""` · `GET {commentId}` · `PUT {commentId}` · `DELETE {commentId}` |
| 8 | `TaskAttachmentsController` | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/attachments` | `POST ""` · `GET ""` · `GET {attachmentId}` · `GET by-name/{name}` · `DELETE {attachmentId}` |
| 9 | `ReportsController` | `/api/workspaces/{workSpaceId}/reports` | `GET projects/{projectId}/tasks-by-priority` · `GET projects/{projectId}/tasks-by-status` · `GET members/{memberId}/performance` · `GET projects/{projectId}/members/{memberId}/performance` · `GET ""` · `GET pdf` |
| 10 | `NotificationsController` | `/api/notifications` | `GET {id}` · `GET all` · `GET all/unread` · `PUT {id}/read` |
| 11 | `WorkSpaceUserDashboardController` | `/api/workspaces/{workspaceId}/dashboard` | `GET ""` |

---

### Controller API tables

#### AuthController — `/api/auth`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register-user` | Register a new regular user |
| POST | `/api/auth/register-admin` | Register a new Admin user (**Admin**) |
| POST | `/api/auth/confirm-email` | Confirm the email address |
| POST | `/api/auth/login` | Log in, sets auth cookies |
| POST | `/api/auth/refresh-token` | Rotate the access token |
| POST | `/api/auth/logout` | Log out, clears cookies |
| GET | `/api/auth` | Get the current user |
| PUT | `/api/auth` | Update the current user's profile |
| POST | `/api/auth/forget-password/send-otp` | Send password-reset OTP |
| POST | `/api/auth/forget-password/resend-otp` | Resend password-reset OTP |
| POST | `/api/auth/forget-password` | Reset password via OTP |
| POST | `/api/auth/reset-password/send-email` | Send password-reset email (token) |
| POST | `/api/auth/reset-password` | Reset password via emailed token |
| POST | `/api/auth/change-email/send-email` | Send change-email confirmation |
| POST | `/api/auth/change-email` | Confirm email change via token |
| POST | `/api/auth/delete-account/send-otp` | Send delete-account OTP |
| POST | `/api/auth/delete-account/resend-otp` | Resend delete-account OTP |
| DELETE | `/api/auth/delete-account` | Permanently delete the account |
| GET | `/api/auth/login-user-with-google` | Start Google OAuth login |
| GET | `/api/auth/login-user-by-provider-callback` | OAuth callback, completes login |

#### UsersController — `/api/users`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/users/{id}` | Get a user by ID |
| GET | `/api/users/all` | List all users (**Admin**) |
| DELETE | `/api/users/{id}` | Delete a user (**Admin**) |

#### WorkSpacesController — `/api/workspaces`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/workspaces/{id}` | Get a workspace by ID |
| GET | `/api/workspaces/all` | List workspaces (Admin: all; user: mine) |
| GET | `/api/workspaces/{id}/all-users` | List workspace members |
| POST | `/api/workspaces` | Create a workspace |
| PUT | `/api/workspaces/{id}` | Update a workspace (**Admin/Owner**) |
| DELETE | `/api/workspaces/{id}` | Delete a workspace (**Admin/Owner**) |

#### WorkSpaceInvitesController — `/api/workspace-invites`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/workspace-invites/{id}` | Get an invite by ID |
| GET | `/api/workspace-invites/all-my-invites` | List invites received by me |
| GET | `/api/workspace-invites/all-my-send-invites` | List invites sent by me |
| POST | `/api/workspace-invites` | Invite a user to a workspace (**Owner**) |
| DELETE | `/api/workspace-invites/{id}` | Delete a pending invite |
| PATCH | `/api/workspace-invites/{id}/accept` | Accept an invite |
| PATCH | `/api/workspace-invites/{id}/reject` | Reject an invite |

#### ProjectsController — `/api/workspaces/{workspaceId}/projects`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/workspaces/{workspaceId}/projects` | Create a project (**Admin/Owner/ProjectManager**) |
| GET | `/api/workspaces/{workspaceId}/projects/{projectId}` | Get a project by ID |
| GET | `/api/workspaces/{workspaceId}/projects` | List projects in the workspace |
| PUT | `/api/workspaces/{workspaceId}/projects/{projectId}` | Update a project (**Admin/Owner/ProjectManager**) |
| PATCH | `/api/workspaces/{workspaceId}/projects/{projectId}/status` | Update project status (**Admin/Owner/ProjectManager**) |
| DELETE | `/api/workspaces/{workspaceId}/projects/{projectId}` | Delete a project (**Admin/Owner/ProjectManager**) |

#### ProjectsTasksController — `/api/workspaces/{workspaceId}/projects/{projectId}/tasks`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks` | Create a task (**Admin/Owner/ProjectManager**) |
| GET | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}` | Get a task by ID |
| GET | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/me` | Get a task assigned to me |
| GET | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks` | List project tasks (filterable) |
| GET | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/users/{userId}` | List a user's tasks |
| GET | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/me` | List my tasks |
| PUT | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}` | Update a task (**Admin/Owner/ProjectManager**) |
| DELETE | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}` | Delete a task (**Admin/Owner/ProjectManager**) |
| POST | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/assignments` | Assign a user to the task (**Admin/Owner/ProjectManager**) |
| DELETE | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/assignments/{assignedUserId}` | Unassign a user (**Admin/Owner/ProjectManager**) |
| PATCH | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/status` | Change task status (**Admin/Owner/ProjectManager**) |
| PATCH | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/me/status` | Change my assigned task status |

#### TaskCommentsController — `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/comments`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/comments` | Add a comment |
| GET | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/comments` | List task comments |
| GET | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/comments/{commentId}` | Get a comment by ID |
| PUT | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/comments/{commentId}` | Update a comment (author) |
| DELETE | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/comments/{commentId}` | Delete a comment (Admin/Owner or author) |

#### TaskAttachmentsController — `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/attachments`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/attachments` | Upload an attachment (**Admin/Owner/ProjectManager**) |
| GET | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/attachments` | List task attachments |
| GET | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/attachments/{attachmentId}` | Get attachment by ID |
| GET | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/attachments/by-name/{name}` | Get attachment by file name |
| DELETE | `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/attachments/{attachmentId}` | Delete an attachment (**Admin/Owner/ProjectManager**) |

#### ReportsController — `/api/workspaces/{workSpaceId}/reports`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/workspaces/{workSpaceId}/reports/projects/{projectId}/tasks-by-priority` | Tasks grouped by priority |
| GET | `/api/workspaces/{workSpaceId}/reports/projects/{projectId}/tasks-by-status` | Tasks grouped by status |
| GET | `/api/workspaces/{workSpaceId}/reports/members/{memberId}/performance` | Member performance in workspace (**Admin/Owner/ProjectManager**) |
| GET | `/api/workspaces/{workSpaceId}/reports/projects/{projectId}/members/{memberId}/performance` | Member performance in project |
| GET | `/api/workspaces/{workSpaceId}/reports` | Workspace overview report (**Admin/Owner/ProjectManager**) |
| GET | `/api/workspaces/{workSpaceId}/reports/pdf` | Download workspace report PDF (**Admin/Owner/ProjectManager**) |

#### NotificationsController — `/api/notifications`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/notifications/{id}` | Get a notification by ID |
| GET | `/api/notifications/all` | List my notifications |
| GET | `/api/notifications/all/unread` | List my unread notifications |
| PUT | `/api/notifications/{id}/read` | Mark a notification as read |

#### WorkSpaceUserDashboardController — `/api/workspaces/{workspaceId}/dashboard`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/workspaces/{workspaceId}/dashboard` | Workspace dashboard (Admin/Owner/ProjectManager: full workspace; other members: user-specific) |

---

### 1. Auth — `/api/auth`

#### 1.1 POST `/api/auth/register-user`
Register a new regular user. **Anonymous.**

**Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "password": "Password123!",
  "dateOfBirth": "2000-01-01"
}
```
**Response:** `200 OK` with `{ "id": "<userId>" }`. A confirmation email is sent.

#### 1.2 POST `/api/auth/register-admin`
Register a new **Admin** user. **Admin only.**

Same body as registration. **Response:** `200 OK` with `{ "id": "<userId>" }`.

#### 1.3 POST `/api/auth/confirm-email?email=&token=`
Confirm the email address after registration. **Anonymous.**

**Response:** `204 No Content`.

#### 1.4 POST `/api/auth/login`
Log in and set the auth cookies. **Anonymous.**

**Body:**
```json
{ "email": "john@example.com", "password": "Password123!" }
```
**Response:** `204 No Content` — sets `access_token` and `refresh_token` HttpOnly cookies.

#### 1.5 POST `/api/auth/refresh-token`
Rotate the access token using the `refresh_token` cookie. **Anonymous.**

**Response:** `204 No Content` — refreshes the `access_token` cookie. `401 Unauthorized` if no/invalid refresh token.

#### 1.6 POST `/api/auth/logout`
Log out and clear the auth cookies. **🔒**

**Response:** `204 No Content`.

#### 1.7 GET `/api/auth`
Get the currently authenticated user. **🔒**

**Response:** `200 OK` with a `UserDto`:
```json
{
  "id": "a1b2c3...",
  "email": "john@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "dateOfBirth": "2000-01-01"
}
```

#### 1.8 PUT `/api/auth`
Update the current user's profile. **🔒**

**Body:**
```json
{ "firstName": "Johnny", "lastName": "Doe", "dateOfBirth": "2000-01-01" }
```
**Response:** `201 Created` with the updated `UserDto`.

#### 1.9 POST `/api/auth/forget-password/send-otp`
Send an OTP to the user's email for password reset. **Anonymous.**

**Body:** `{ "email": "john@example.com" }`
**Response:** `204 No Content`.

#### 1.10 POST `/api/auth/forget-password/resend-otp`
Resend the password-reset OTP. **Anonymous.**

**Body:** `{ "email": "john@example.com" }`
**Response:** `204 No Content`.

#### 1.11 POST `/api/auth/forget-password`
Reset the password using the OTP (for users who forgot it). **Anonymous.**

**Body:**
```json
{ "email": "john@example.com", "newPassword": "NewPassword123!", "otp": "123456" }
```
**Response:** `204 No Content`.

#### 1.12 POST `/api/auth/reset-password/send-email`
Send a password-reset email (token link) to the authenticated user. **🔒**

**Response:** `204 No Content`.

#### 1.13 POST `/api/auth/reset-password`
Reset the password with the emailed token. **🔒**

**Body:** `{ "token": "<resetToken>", "newPassword": "NewPassword123!" }`
**Response:** `204 No Content`.

#### 1.14 POST `/api/auth/change-email/send-email?email=`
Send a change-email confirmation to the new address. **🔒**

**Response:** `204 No Content`.

#### 1.15 POST `/api/auth/change-email`
Confirm the email change with the emailed token. **🔒**

**Body:** `{ "token": "<changeToken>", "newEmail": "new@example.com" }`
**Response:** `204 No Content`.

#### 1.16 POST `/api/auth/delete-account/send-otp`
Send an OTP for account deletion. **🔒**

**Body:** `{ "email": "john@example.com" }`
**Response:** `204 No Content`.

#### 1.17 POST `/api/auth/delete-account/resend-otp`
Resend the delete-account OTP. **🔒**

**Body:** `{ "email": "john@example.com" }`
**Response:** `204 No Content`.

#### 1.18 DELETE `/api/auth/delete-account`
Permanently delete the current user's account using the OTP. **Anonymous (requires auth cookie).**

**Body:** `{ "email": "john@example.com", "otp": "123456" }`
**Response:** `204 No Content` — clears auth cookies.

#### 1.19 GET `/api/auth/login-user-with-google?returnUrl=`
Start Google OAuth login. **Anonymous.** `returnUrl` must be an allowed origin.

**Response:** `401 Challenge` redirecting to Google's consent screen.

#### 1.20 GET `/api/auth/login-user-by-provider-callback?returnUrl=&remoteError=`
OAuth callback endpoint. **Anonymous.**

**Response:** `302 Redirect` to `returnUrl` with auth cookies set. Returns `400` for invalid `returnUrl` or remote errors.

---

### 2. Users — `/api/users`

#### 2.1 GET `/api/users/{id}` 🔒
Get a user by ID.

**Response:** `200 OK` with a `UserDto` (see [1.7](#17-get-apiauth)).

#### 2.2 GET `/api/users/all?pageNumber=&pageSize=` 🔒 **Admin**
List all users (paginated).

**Response:** `200 OK` with a `PaginationResultDto<UserDto>`.

#### 2.3 DELETE `/api/users/{id}` 🔒 **Admin**
Delete a user account.

**Response:** `204 No Content`.

---

### 3. Workspaces — `/api/workspaces`

#### 3.1 GET `/api/workspaces/{id}` 🔒
Get a workspace by ID. Accessible by **Admin** or any workspace member.

**Response:** `200 OK` with a `WorkSpaceDto`:
```json
{
  "id": 1,
  "name": "Acme Corp",
  "description": "Product development",
  "createdById": "a1b2c3...",
  "createdAt": "2026-01-01T10:00:00Z",
  "lastUpdatedById": null,
  "lastUpdatedAt": null
}
```

#### 3.2 GET `/api/workspaces/all?pageNumber=&pageSize=` 🔒
List workspaces (paginated).
- **Admin:** returns **all** workspaces (`GetAllWorkSpacesQuery`).
- **Any user:** returns only the workspaces the user belongs to (`GetAllUserWorkSpacesQuery`).

**Response:** `200 OK` with `PaginationResultDto<WorkSpaceDto>`.

#### 3.3 GET `/api/workspaces/{id}/all-users?pageNumber=&pageSize=` 🔒
List the members of a workspace. Accessible by **Admin** or any workspace member.

**Response:** `200 OK` with `PaginationResultDto<WorkSpaceUserDto>`:
```json
{
  "id": 1,
  "fullName": "John Doe",
  "email": "john@example.com",
  "workSpaceRole": "Owner"
}
```

#### 3.4 POST `/api/workspaces` 🔒
Create a workspace. The creator becomes its **Owner**.

**Body:**
```json
{ "name": "Acme Corp", "description": "Product development" }
```
**Response:** `201 Created` with the `WorkSpaceDto` and a `Location` header to `GET /api/workspaces/{id}`.

#### 3.5 PUT `/api/workspaces/{id}` 🔒 **Admin or Owner**
Update a workspace.

**Body:** `{ "name": "Acme Corp 2", "description": "Updated" }`
**Response:** `204 No Content`.

#### 3.6 DELETE `/api/workspaces/{id}` 🔒 **Admin or Owner**
Delete (soft-delete) a workspace.

**Response:** `204 No Content`.

---

### 4. Workspace Invites — `/api/workspace-invites`

#### 4.1 GET `/api/workspace-invites/{id}` 🔒
Get an invite by ID.
- **Admin:** any invite.
- **Other users:** only invites addressed to them.

**Response:** `200 OK` with a `WorkSpaceInviteDto`:
```json
{
  "id": 1,
  "workSpaceId": 2,
  "invitedToId": "a1b2c3...",
  "invitedById": "d4e5f6...",
  "createdAt": "2026-01-01T10:00:00Z",
  "expiresAt": "2026-03-01T10:00:00Z",
  "workSpaceInviteStatus": "Pending"
}
```

#### 4.2 GET `/api/workspace-invites/all-my-invites?pageNumber=&pageSize=` 🔒
List invites **received** by the current user (paginated).

**Response:** `200 OK` with `PaginationResultDto<WorkSpaceInviteDto>`.

#### 4.3 GET `/api/workspace-invites/all-my-send-invites?pageNumber=&pageSize=` 🔒
List invites **sent** by the current user (paginated).

**Response:** `200 OK` with `PaginationResultDto<WorkSpaceInviteDto>`.

#### 4.4 POST `/api/workspace-invites` 🔒 **Workspace Owner**
Invite a user to a workspace with a role.

**Body:**
```json
{ "workSpaceId": 2, "inviteToEmail": "jane@example.com", "workSpaceRole": "ProjectManager" }
```
**Response:** `201 Created` with the `WorkSpaceInviteDto` and a `Location` header to `GET /api/workspace-invites/{id}`.

#### 4.5 DELETE `/api/workspace-invites/{id}` 🔒
Delete a pending invite (by the sender).

**Response:** `204 No Content`.

#### 4.6 PATCH `/api/workspace-invites/{id}/accept` 🔒
Accept an invite (must be the invited user). Adds the user to the workspace with the invited role.

**Response:** `204 No Content`.

#### 4.7 PATCH `/api/workspace-invites/{id}/reject` 🔒
Reject an invite (must be the invited user).

**Response:** `204 No Content`.

---

### 5. Projects — `/api/workspaces/{workspaceId}/projects`

Authorization helper:
- **Manage** (create/update/status/delete): **Admin**, **Owner**, or **ProjectManager**.
- **Read** (get/list): **Admin** or any workspace member.

#### 5.1 POST `` 🔒 *Manage*
Create a project in the workspace.

**Body:**
```json
{ "name": "Website Redesign", "description": "Q1 initiative" }
```
**Response:** `201 Created` with the `ProjectDto`:
```json
{
  "id": 10,
  "name": "Website Redesign",
  "description": "Q1 initiative",
  "status": "Active",
  "workSpaceId": 2,
  "createdById": "a1b2c3...",
  "createdAt": "2026-01-10T09:00:00Z",
  "lastUpdatedById": null,
  "lastUpdatedAt": null
}
```

#### 5.2 GET `/{projectId}` 🔒 *Read*
Get a project by ID.

**Response:** `200 OK` with a `ProjectDto`.

#### 5.3 GET `` 🔒 *Read*
List projects in a workspace (paginated).

**Response:** `200 OK` with `PaginationResultDto<ProjectDto>`.

#### 5.4 PUT `/{projectId}` 🔒 *Manage*
Update a project.

**Body:**
```json
{ "name": "Website Redesign v2", "description": "Updated", "status": "OnHold" }
```
**Response:** `204 No Content`.

#### 5.5 PATCH `/{projectId}/status` 🔒 *Manage*
Update only the project status.

**Body:** `{ "status": "Completed" }`
**Response:** `204 No Content`.

#### 5.6 DELETE `/{projectId}` 🔒 *Manage*
Delete (soft-delete) a project.

**Response:** `204 No Content`.

---

### 6. Tasks — `/api/workspaces/{workspaceId}/projects/{projectId}/tasks`

Authorization helper:
- **Manage** (create/update/delete/assign/status): **Admin**, **Owner**, or **ProjectManager**.
- **Read / personal actions**: **Admin** or any workspace member.

#### 6.1 POST `` 🔒 *Manage*
Create a task. An assignment to `assignedUserId` is created immediately.

**Body:**
```json
{
  "name": "Design landing page",
  "description": "High-fidelity mockups",
  "deadline": "2026-03-01T18:00:00Z",
  "priority": "High",
  "assignedUserId": "a1b2c3..."
}
```
**Response:** `201 Created` with the `TaskDto`:
```json
{
  "id": 100,
  "name": "Design landing page",
  "description": "High-fidelity mockups",
  "deadline": "2026-03-01T18:00:00Z",
  "taskStatus": "Backlog",
  "taskPriority": "High",
  "createdAt": "2026-01-15T08:00:00Z",
  "lastUpdatedAt": null,
  "lastUpdatedById": null,
  "projectId": 10,
  "createdById": "a1b2c3...",
  "assignments": [
    {
      "id": 500,
      "assignedToId": "a1b2c3...",
      "assignedById": "d4e5f6...",
      "createdAt": "2026-01-15T08:00:00Z",
      "unassignedAt": null,
      "isActive": true
    }
  ],
  "attachments": []
}
```

#### 6.2 GET `/{taskId}` 🔒 *Read*
Get a task by ID.

**Response:** `200 OK` with a `TaskDto`.

#### 6.3 GET `/{taskId}/me` 🔒 *Workspace member*
Get a task **only if it is assigned to the current user** (or the user is admin).

**Response:** `200 OK` with a `TaskDto`.

#### 6.4 GET `` 🔒 *Read*
List all tasks in a project with **pagination and filtering**.

**Query params:**
| Param | Type | Description |
|-------|------|-------------|
| `pageNumber` | int | Page number (default `1`) |
| `pageSize` | int | Page size (default `10`) |
| `status` | enum | Filter by `ProjectTaskStatus` |
| `priority` | enum | Filter by `TaskPriority` |
| `searchTerm` | string | Search by name/description |
| `sortBy` | string | Sort field |
| `sortOrder` | string | `asc` / `desc` |

**Response:** `200 OK` with `PaginationResultDto<TaskDto>`.

#### 6.5 GET `/users/{userId}` 🔒 *Read*
List the tasks assigned to a specific user (same filter params as 6.4).

**Response:** `200 OK` with `PaginationResultDto<TaskDto>`.

#### 6.6 GET `/me` 🔒 *Workspace member*
List the tasks assigned to the current user (same filter params as 6.4).

**Response:** `200 OK` with `PaginationResultDto<TaskDto>`.

#### 6.7 PUT `/{taskId}` 🔒 *Manage*
Update a task.

**Body:**
```json
{ "name": "Design landing page (v2)", "description": "Updated", "deadline": "2026-03-05T18:00:00Z", "priority": "Critical" }
```
**Response:** `200 OK` with the updated `TaskDto`.

#### 6.8 DELETE `/{taskId}` 🔒 *Manage*
Delete (soft-delete) a task.

**Response:** `204 No Content`.

#### 6.9 POST `/{taskId}/assignments` 🔒 *Manage*
Assign a user to the task.

**Body:** `{ "userId": "a1b2c3..." }`
**Response:** `200 OK` with `{ "assignments": [ <TaskAssignmentDto> ] }`.

#### 6.10 DELETE `/{taskId}/assignments/{assignedUserId}` 🔒 *Manage*
Unassign a user from the task.

**Response:** `204 No Content`.

#### 6.11 PATCH `/{taskId}/status` 🔒 *Manage*
Change the task status (performed by a manager).

**Body:** `{ "status": "InProgress" }`
**Response:** `200 OK` with the updated `TaskDto`.

#### 6.12 PATCH `/{taskId}/me/status` 🔒 *Workspace member*
Change the task status **when the task is assigned to the current user** (self-service).

**Body:** `{ "status": "Done" }`
**Response:** `200 OK` with the updated `TaskDto`.

---

### 7. Comments — `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/comments`

All comment actions require the user to be a workspace member.

#### 7.1 POST `` 🔒
Add a comment to the task.

**Body:** `{ "comment": "Started working on this." }`
**Response:** `201 Created` with the `TaskCommentDto`:
```json
{
  "id": 900,
  "comment": "Started working on this.",
  "taskId": 100,
  "commentById": "a1b2c3...",
  "commentByName": "John Doe",
  "createdAt": "2026-01-16T12:00:00Z",
  "lastUpdatedAt": null
}
```

#### 7.2 GET `` 🔒
List comments for the task (paginated).

**Response:** `200 OK` with `PaginationResultDto<TaskCommentDto>`.

#### 7.3 GET `/{commentId}` 🔒
Get a single comment.

**Response:** `200 OK` with a `TaskCommentDto`.

#### 7.4 PUT `/{commentId}` 🔒
Update a comment (must be the comment author).

**Body:** `{ "comment": "Updated text" }`
**Response:** `200 OK` with the updated `TaskCommentDto`.

#### 7.5 DELETE `/{commentId}` 🔒
Delete a comment.
- **Admin/Owner** may delete any comment.
- Otherwise, the **comment author** may delete their own comment.

**Response:** `204 No Content`.

---

### 8. Attachments — `/api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/attachments`

#### 8.1 POST `` 🔒 *Admin/Owner/ProjectManager*
Upload a file attachment (`multipart/form-data`).

| Field | Value |
|-------|-------|
| `File` | The file to upload (form-data file field) |

**Constraints:** `.pdf`, `.jpg`, `.jpeg`, `.png`; max **50 MB** (`RequestSizeLimit`/`RequestFormLimits` set to 52,428,800 bytes).

**Response:** `201 Created` with the `TaskAttachmentDto`:
```json
{
  "id": 700,
  "name": "wireframe.pdf",
  "url": "/uploads/workspaces/2/tasks/100/wireframe.pdf",
  "createdAt": "2026-01-16T14:00:00Z"
}
```

#### 8.2 GET `` 🔒
List all attachments of the task.

**Response:** `200 OK` with `List<TaskAttachmentDto>`.

#### 8.3 GET `/{attachmentId}` 🔒
Get attachment metadata by ID.

**Response:** `200 OK` with a `TaskAttachmentDto`.

#### 8.4 GET `/by-name/{name}` 🔒
Get an attachment by file name.

**Response:** `200 OK` with a `TaskAttachmentDto`.

#### 8.5 DELETE `/{attachmentId}` 🔒 *Admin/Owner/ProjectManager*
Delete an attachment.

**Response:** `204 No Content`.

---

### 9. Reports — `/api/workspaces/{workSpaceId}/reports`

Authorization helper:
- **Manage reports** (workspace report, PDF, member performance in workspace): **Admin**, **Owner**, or **ProjectManager**.
- **View reports** (project task breakdowns, member performance in project): **Admin** or any workspace member.

Report queries are cached in Redis for 10 minutes.

#### 9.1 GET `/projects/{projectId}/tasks-by-priority` 🔒 *View*
Tasks grouped by priority.

**Response:** `200 OK` with:
```json
[ { "taskPriority": "Low", "count": 3 }, { "taskPriority": "Critical", "count": 1 } ]
```

#### 9.2 GET `/projects/{projectId}/tasks-by-status` 🔒 *View*
Tasks grouped by status.

**Response:** `200 OK` with:
```json
[ { "taskStatus": "Backlog", "count": 4 }, { "taskStatus": "Done", "count": 2 } ]
```

#### 9.3 GET `/members/{memberId}/performance` 🔒 *Manage*
Member performance across the whole workspace.

**Response:** `200 OK` with a `MemberPerformance`:
```json
{
  "id": "a1b2c3...",
  "name": "John Doe",
  "assignedCount": 12,
  "inProgressCount": 3,
  "doneCount": 7
}
```

#### 9.4 GET `/projects/{projectId}/members/{memberId}/performance` 🔒 *View*
Member performance within a single project.

**Response:** `200 OK` with a `MemberPerformance`.

#### 9.5 GET `` 🔒 *Manage*
Full workspace overview report.

**Response:** `200 OK` with a `WorkSpaceReportDto`:
```json
{
  "workSpaceName": "Acme Corp",
  "ownerNames": [ "John Doe" ],
  "totalProjects": 4,
  "totalMembers": 8,
  "totalTasks": 25,
  "totalBacklogTasks": 5,
  "totalTodoTasks": 6,
  "totalInProgressTasks": 4,
  "totalReviewTasks": 2,
  "totalDoneTasks": 8,
  "memberPerformances": [ { "id": "...", "name": "John Doe", "assignedCount": 12, "inProgressCount": 3, "doneCount": 7 } ]
}
```

#### 9.6 GET `/pdf` 🔒 *Manage*
Download the workspace report as a PDF.

**Response:** `200 OK` — `application/pdf` file named `workspace-report.pdf`.

---

### 10. Notifications — `/api/notifications`

All notification endpoints act on the **current user's** notifications (via the auth cookie).

#### 10.1 GET `/{id}` 🔒
Get a notification by ID (must belong to the current user).

**Response:** `200 OK` with a `NotificationDto`:
```json
{
  "id": 300,
  "notifyToId": "a1b2c3...",
  "taskId": 100,
  "workSpaceInviteId": null,
  "title": "New task assigned",
  "message": "You were assigned to 'Design landing page'",
  "createdAt": "2026-01-15T08:00:00Z",
  "isRead": false,
  "readAt": null,
  "notificationType": "TaskAssigned"
}
```

#### 10.2 GET `/all?pageNumber=&pageSize=` 🔒
List all notifications for the current user (paginated).

**Response:** `200 OK` with `PaginationResultDto<NotificationDto>`.

#### 10.3 GET `/all/unread?pageNumber=&pageSize=` 🔒
List only unread notifications (paginated).

**Response:** `200 OK` with `PaginationResultDto<NotificationDto>`.

#### 10.4 PUT `/{id}/read` 🔒
Mark a notification as read.

**Response:** `204 No Content`.

---

### 11. Workspace User Dashboard — `/api/workspaces/{workspaceId}/dashboard`

#### 11.1 GET `` 🔒
Get a combined dashboard for the workspace:
- **Admin, Owner, or ProjectManager:** returns the **full workspace** dashboard (stats, status/priority breakdowns, active tasks across the workspace).
- **Other members:** returns a **user-specific** dashboard filtered to the current user's data.

The dashboard is cached in Redis for **5 minutes** (key `WorkSpaceDashboard:{workspaceId}:{userId}`).

**Response:** `200 OK` with a `WorkSpaceDashboardDto`:
```json
{
  "workspace": { "id": 2, "name": "Acme Corp" },
  "stats": {
    "totalProjects": 4,
    "totalTasks": 25,
    "inProgressTasks": 4,
    "completedTasks": 8,
    "completionRate": 32.0
  },
  "tasksByStatusReportDtos": [ { "taskStatus": "Backlog", "count": 5 }, { "taskStatus": "Done", "count": 8 } ],
  "tasksByPriorityReportDtos": [ { "taskPriority": "High", "count": 3 }, { "taskPriority": "Low", "count": 1 } ],
  "activeTasks": [
    {
      "id": 100,
      "name": "Design landing page",
      "projectName": "Website Redesign",
      "priority": "High",
      "status": "InProgress",
      "createdAt": "2026-01-15T08:00:00Z",
      "deadLine": "2026-03-01T18:00:00Z"
    }
  ],
  "unReadNotifications": [ /* NotificationDto items (max 10) */ ]
}
```

For dashboard KPI/breakdown details, see also [9. Reports](#9-reports--apiworkspacesworkspaceidreports).

---

## Pagination

Any list endpoint accepts `PaginationRequestDto` via query string:

| Query param | Type | Default |
|-------------|------|---------|
| `pageNumber` | int | `1` |
| `pageSize` | int | `10` |

All paginated responses use the standard envelope `PaginationResultDto<T>`:

```json
{
  "data": [ /* items of type T */ ],
  "totalCount": 42,
  "pageNumber": 1,
  "pageSize": 10,
  "nextPage": 2,
  "previousPage": null,
  "totalPages": 5,
  "hasNextPage": true,
  "hasPreviousPage": false
}
```

Affected endpoints: workspace lists (3.2, 3.3), invites (4.2, 4.3), users (2.2), projects (5.3), tasks (6.4, 6.5, 6.6), comments (7.2), notifications (10.2, 10.3).

---

## Real-time Notifications (SignalR)

Connect to the hub at **`/notificationHub`**.

**Hub methods (client → server):**

| Method | Params | Description |
|--------|--------|-------------|
| `JoinWorkSpace` | `workSpaceId` (long) | Join the group `workspace-{workSpaceId}` to receive its notifications |
| `LeaveWorkSpace` | `workSpaceId` (long) | Leave the group |

**Client method (server → client):**

| Method | Payload | Description |
|--------|---------|-------------|
| `ReceiveNotification` | `NotificationDto` | A new notification for the user/workspace |

The server sends notifications either to a specific user (`Clients.User(userId)`) or to a workspace group (`Clients.Group("workspace-{id}")`), e.g. when a task is assigned, its status changes, a comment is added, or an invite is created.

---

## Error Handling

- The **Application** layer returns `ErrorOr<T>`; controllers translate failures to **RFC 7807 Problem Details** via `errors.ToProblemDetailsObjectResult()`.
- A global exception handler (`src/Api/ExceptionHandler/GlobalExceptionHandler.cs`) catches and maps infrastructure exceptions:
  - `UniqueConstraintViolationException`
  - `ForeignKeyConstraintViolationException`
  - `DatabaseOperationException`
  - `CacheOperationException`
- Expected failures (e.g. "not found", "forbidden", "validation") never throw — they are returned as errors.

---

## Project Structure

```
TaskManagments/
├── src/
│   ├── Api/                      # Presentation layer
│   │   ├── Controllers/          # 11 API controllers
│   │   ├── Hubs/Notification/    # SignalR hub + client interface + service
│   │   ├── Polices/WorkSpace/    # Authorization requirement handlers
│   │   ├── Common/               # Extensions, origins, file URL service
│   │   └── ExceptionHandler/     # Global exception handler
│   ├── Application/              # CQRS features
│   │   ├── Common/               # DTOs, errors, exceptions, interfaces
│   │   └── Features/             # Auth, Users, WorkSpaces, WorkSpaceUsers,
│   │                             # WorkSpaceInvites, Projects, Tasks,
│   │                             # TaskComments, TaskAttachments, Reports,
│   │                             # Notifications, WorkSpaceUserDashboard
│   ├── Domain/                   # Pure domain
│   │   ├── Common/               # Enums, interfaces
│   │   └── Entities/             # 11 entities
│   └── Infrastructure/           # EF Core, Redis, Identity, Mail, services
├── .opencode/                    # Repo conventions & dev rules
└── TaskManagments.sln
```

### Feature layout (CQRS example)

```
Features/
  WorkSpaces/
    commands/
      CreateWorkSpace/
        CreateWorkSpaceCommand.cs
        CreateWorkSpaceCommandHandler.cs
        CreateWorkSpaceCommandValidator.cs
        CreateWorkSpaceDto.cs
    queries/
      GetWorkSpaceById/
        GetWorkSpaceByIdQuery.cs
        GetWorkSpaceByIdQueryHandler.cs
        GetWorkSpaceByIdQueryValidator.cs
    WorkSpaceDto.cs
```

---

## License

This project is licensed under the [MIT License](LICENSE).
