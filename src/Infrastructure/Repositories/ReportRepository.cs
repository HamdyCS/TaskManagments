using Application.Common.Dtos;
using Application.Common.Dtos.WorkSpacesOverview;
using Application.Common.Interfaces.Repositories;
using Domain.Common.Enums;
using Domain.Common.Pagination;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class ReportRepository(AppDbContext context) : IReportRepository
    {
        public async Task<IEnumerable<TasksByStatusReportDto>> GetProjectTasksReportByStatusAsync(long projectId)
        {
            var ProjectTasksReportDtoList = await context.ProjectTasks
                .Where(t => t.ProjectId == projectId)
                .GroupBy(t => t.TaskStatus)
                .Select(g => new
                 TasksByStatusReportDto
                { TaskStatus = g.Key, Count = g.Count() })
                .ToListAsync();

            return ProjectTasksReportDtoList;
        }

        public async Task<IEnumerable<TasksByPriorityReportDto>> GetProjectTasksReportByPriorityAsync(long projectId)
        {
            var ProjectTasksReportDtoList = await context.ProjectTasks
                .Where(t => t.ProjectId == projectId)
                .GroupBy(t => t.TaskPriority)
                .Select(g => new
                 TasksByPriorityReportDto
                { TaskPriority = g.Key, Count = g.Count() })
                .ToListAsync();

            return ProjectTasksReportDtoList;
        }

        public async Task<PaginationResult<MemberPerformanceDto>> GetAllMemberPerformancesAsync(int pageNumber, int pageSize, string? memberNameQuery)
        {
            var query = context.Users.AsQueryable();
            var trimedQuery = memberNameQuery?.Trim();
            if (!string.IsNullOrEmpty(memberNameQuery))
            {
                query = query.Where(u => (u.FirstName + " " + u.LastName).Contains(trimedQuery)
                          || (u.LastName + " " + u.FirstName).Contains(trimedQuery));
            }
            var totalCount = await query.CountAsync();
            var memberPerformances = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new MemberPerformanceDto
                {
                    Id = u.Id,
                    Name = u.FirstName + " " + u.LastName,
                    AssignedCount = context.TaskAssignments.Count(ta => ta.AssignedToId == u.Id && ta.IsActive),
                    InProgressCount = context.TaskAssignments.Count(ta => ta.AssignedToId == u.Id && ta.IsActive && ta.Task.TaskStatus == ProjectTaskStatus.InProgress),
                    DoneCount = context.TaskAssignments.Count(ta => ta.AssignedToId == u.Id && ta.IsActive && ta.Task.TaskStatus == ProjectTaskStatus.Done)
                })
                .ToListAsync();
            foreach (var memberPerformance in memberPerformances)
            {
                memberPerformance.CompletionPercentage = memberPerformance.AssignedCount > 0
                    ? (double)memberPerformance.DoneCount / memberPerformance.AssignedCount * 100
                    : 0;
            }
            return new PaginationResult<MemberPerformanceDto>(memberPerformances, totalCount, pageNumber, pageSize);
        }

        public async Task<MemberPerformanceDto> GetMemberPerformanceInWorkSpaceAsync(long workspaceId)
        {

            var memberPerformance = await context.TaskAssignments
              .Where(ta => ta.Task.Project.WorkSpaceId == workspaceId && ta.IsActive)
              .GroupBy(ta => ta.AssignedToId)
              .Select(g => new MemberPerformanceDto
              {
                  Id = g.Key,
                  Name = g.First().AssignedTo.FirstName + " " + g.First().AssignedTo.LastName,
                  AssignedCount = g.Count(),
                  InProgressCount = g.Count(ta => ta.Task.TaskStatus == ProjectTaskStatus.InProgress),
                  DoneCount = g.Count(ta => ta.Task.TaskStatus == ProjectTaskStatus.Done)
              }).FirstOrDefaultAsync();

            if(memberPerformance is null)
            {
                return new MemberPerformanceDto { AssignedCount = 0, InProgressCount = 0, DoneCount = 0 };
            }


            memberPerformance.CompletionPercentage = memberPerformance.AssignedCount > 0
                ? (double)memberPerformance.DoneCount / memberPerformance.AssignedCount * 100
                : 0;
            return memberPerformance;
        }

        public async Task<IEnumerable<MemberPerformanceDto>> GetAllMemberPerformanceInWorkSpaceAsync(long workspaceId)
        {

            var memberPerformances = await context.WorkSpaceUsers
              .Where(wu => wu.WorkSpaceId == workspaceId)
              .Select(wu => new MemberPerformanceDto
              {
                  Id = wu.UserId,
                  Name = wu.User.FirstName + " " + wu.User.LastName,

                  AssignedCount = context.TaskAssignments.Count(ta=>ta.AssignedToId == wu.UserId &&
                  ta.IsActive && ta.Task.Project.WorkSpaceId == wu.WorkSpaceId && ta.IsActive),

                  InProgressCount = context.TaskAssignments.Count(ta => ta.AssignedToId == wu.UserId &&
                  ta.IsActive && ta.Task.Project.WorkSpaceId == wu.WorkSpaceId && 
                  ta.Task.TaskStatus == ProjectTaskStatus.InProgress && ta.IsActive),

                  DoneCount = context.TaskAssignments.Count(ta => ta.AssignedToId == wu.UserId &&
                  ta.IsActive && ta.Task.Project.WorkSpaceId == wu.WorkSpaceId &&
                  ta.Task.TaskStatus == ProjectTaskStatus.Done && ta.IsActive),
              }).ToListAsync();

            return memberPerformances;
        }

        public async Task<MemberPerformanceDto> GetMemberPerformanceInProjectAsync(long projectId, string memberId)
        {

            var memberPerformance = await context.TaskAssignments
              .Where(ta => ta.AssignedToId == memberId
              && ta.Task.ProjectId == projectId && ta.IsActive)
              .GroupBy(_ => 1)
              .Select(g => new MemberPerformanceDto
              {
                  Id = g.First().AssignedToId,
                  Name = g.First().AssignedTo.FirstName + " " + g.First().AssignedTo.LastName,
                  AssignedCount = g.Count(),
                  InProgressCount = g.Count(ta => ta.Task.TaskStatus == ProjectTaskStatus.InProgress),
                  DoneCount = g.Count(ta => ta.Task.TaskStatus == ProjectTaskStatus.Done)
              }).FirstOrDefaultAsync();

            return memberPerformance ?? new MemberPerformanceDto { AssignedCount = 0, InProgressCount = 0, DoneCount = 0 };
        }

        public async Task<WorkSpaceReportDto> GetWorkSpaceReportAsync(long workspaceId)
        {
            var workspaceReport = await context.WorkSpaces
                .Where(ws => ws.Id == workspaceId)
                .Select(ws => new WorkSpaceReportDto
                {
                    WorkSpaceName = ws.Name,
                    OwnerNames = ws.WorkSpaceUsers.Where(ws => ws.WorkSpaceRole == WorkSpaceRole.Owner).Select(o => o.User.FirstName + " " + o.User.LastName),
                    TotalProjects = ws.Projects.Count(),
                    TotalMembers = ws.WorkSpaceUsers.Count(),
                    TotalTasks = ws.Projects.SelectMany(p => p.Tasks).Count(),
                    TotalBacklogTasks = ws.Projects.SelectMany(p => p.Tasks).Count(t => t.TaskStatus == ProjectTaskStatus.Backlog),
                    TotalTodoTasks = ws.Projects.SelectMany(p => p.Tasks).Count(t => t.TaskStatus == ProjectTaskStatus.Todo),
                    TotalInProgressTasks = ws.Projects.SelectMany(p => p.Tasks).Count(t => t.TaskStatus == ProjectTaskStatus.InProgress),
                    TotalReviewTasks = ws.Projects.SelectMany(p => p.Tasks).Count(t => t.TaskStatus == ProjectTaskStatus.Review),
                    TotalDoneTasks = ws.Projects.SelectMany(p => p.Tasks).Count(t => t.TaskStatus == ProjectTaskStatus.Done)
                })
                .FirstOrDefaultAsync();

            // If no report is found, return an empty WorkSpaceReportDto with default values

            if (workspaceReport is null)
            {
                return new WorkSpaceReportDto
                {
                    WorkSpaceName = string.Empty,
                    OwnerNames = new List<string>(),
                    TotalProjects = 0,
                    TotalMembers = 0,
                    TotalTasks = 0,
                    TotalBacklogTasks = 0,
                    TotalTodoTasks = 0,
                    TotalInProgressTasks = 0,
                    TotalReviewTasks = 0,
                    TotalDoneTasks = 0,
                    MemberPerformances = new List<MemberPerformanceDto>()
                };
            }

            workspaceReport.CompletionPercentage = workspaceReport.TotalTasks > 0
                ? (double)workspaceReport.TotalDoneTasks / workspaceReport.TotalTasks * 100
                : 0;
            workspaceReport.MemberPerformances = await GetAllMemberPerformanceInWorkSpaceAsync(workspaceId);
            return workspaceReport;
        }

        public async Task<WorkSpacesOverviewReportDto?> GetWorkSpacesOverviewAsync(DateTime? from, DateTime? to)
        {

            var usersQuery = context.Users
                .Where(u => u.RoleId == (int)Role.User);

            var workspacesQuery = context.WorkSpaces.AsQueryable();

            var projectsQuery = context.Projects.AsQueryable();

            var tasksQuery = context.Projects
                .SelectMany(p => p.Tasks)
                .AsQueryable();

            if (from.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.CreatedAt >= from.Value);
                workspacesQuery = workspacesQuery.Where(ws => ws.CreatedAt >= from.Value);
                projectsQuery = projectsQuery.Where(p => p.CreatedAt >= from.Value);
                tasksQuery = tasksQuery.Where(t => t.CreatedAt >= from.Value);
            }

            if (to.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.CreatedAt <= to.Value);
                workspacesQuery = workspacesQuery.Where(ws => ws.CreatedAt <= to.Value);
                projectsQuery = projectsQuery.Where(p => p.CreatedAt <= to.Value);
                tasksQuery = tasksQuery.Where(t => t.CreatedAt <= to.Value);
            }

            return new WorkSpacesOverviewReportDto
            {
                RegularUsersCount = await usersQuery.CountAsync(),

                WorkspacesCount = await workspacesQuery.CountAsync(),

                ProjectsCount = await projectsQuery.CountAsync(),

                TasksCount = await tasksQuery.CountAsync(),

                TasksByPriorityReportDtos = await tasksQuery
                    .GroupBy(t => t.TaskPriority)
                    .Select(g => new TasksByPriorityReportDto
                    {
                        TaskPriority = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync(),

                TasksByStatusReportDtos = await tasksQuery
                    .GroupBy(t => t.TaskStatus)
                    .Select(g => new TasksByStatusReportDto
                    {
                        TaskStatus = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync()
            };
        }

    }
}