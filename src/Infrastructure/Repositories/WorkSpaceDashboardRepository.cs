using Application.Common.Dtos;
using Application.Common.Dtos.WorkSpaceDashboard;
using Application.Common.Dtos.WorkSpaceUserDashboard;
using Application.Common.Interfaces;
using Domain.Common.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class WorkSpaceDashboardRepository(AppDbContext appDbContext) : IWorkSpaceDashboardRepository
    {
        public async Task<WorkSpaceDashboardDto> GetWorkSpaceDashboardAsync(long workspaceId)
        {
            var workspaceDashboardDto = await appDbContext.WorkSpaces
                .Where(w => w.Id == workspaceId)
                .Select(w => new WorkSpaceDashboardDto
                {
                    Workspace = new WorkSpaceDashboardSummaryDto
                    {
                        Id = w.Id,
                        Name = w.Name,
                    },

                    Stats = new WorkSpaceDashboardStatsDto
                    {
                        TotalTasks = w.Projects
                            .SelectMany(p => p.Tasks)
                            .Count(),

                        CompletedTasks = w.Projects
                            .SelectMany(p => p.Tasks)
                            .Count(t => t.TaskStatus == ProjectTaskStatus.Done),

                        InProgressTasks = w.Projects
                            .SelectMany(p => p.Tasks)
                            .Count(t => t.TaskStatus == ProjectTaskStatus.InProgress),

                        TotalProjects = w.Projects.Count(),

                        CompletionRate = w.Projects
                            .SelectMany(p => p.Tasks)
                            .Count() == 0
                                ? 0
                                : w.Projects
                                    .SelectMany(p => p.Tasks)
                                    .Count(t => t.TaskStatus == ProjectTaskStatus.Done)
                                    / (double)w.Projects
                                        .SelectMany(p => p.Tasks)
                                        .Count() * 100
                    },

                    TasksByStatusReportDtos = w.Projects
                        .SelectMany(p => p.Tasks)
                        .GroupBy(t => t.TaskStatus)
                        .Select(g => new TasksByStatusReportDto
                        {
                            TaskStatus = g.Key,
                            Count = g.Count()
                        })
                        .ToList(),

                    TasksByPriorityReportDtos = w.Projects
                        .SelectMany(p => p.Tasks)
                        .GroupBy(t => t.TaskPriority)
                        .Select(g => new TasksByPriorityReportDto
                        {
                            TaskPriority = g.Key,
                            Count = g.Count()
                        })
                        .ToList(),

                    ActiveTasks = w.Projects
                        .SelectMany(p => p.Tasks)
                        .Where(t => t.TaskStatus != ProjectTaskStatus.Done)
                        .Select(t => new WorkSpaceTaskDashboardDto
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Status = t.TaskStatus,
                            ProjectName = t.Project.Name,
                            Priority = t.TaskPriority,
                            CreatedAt = t.CreatedAt,
                            DeadLine = t.Deadline
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (workspaceDashboardDto is null)
            {
                return new WorkSpaceDashboardDto
                {
                    Workspace = new WorkSpaceDashboardSummaryDto
                    {
                        Id = workspaceId,
                        Name = string.Empty
                    },
                    Stats = new WorkSpaceDashboardStatsDto
                    {
                        TotalTasks = 0,
                        CompletedTasks = 0,
                        InProgressTasks = 0,
                        TotalProjects = 0,
                        CompletionRate = 0
                    },
                    TasksByStatusReportDtos = new List<TasksByStatusReportDto>(),
                    TasksByPriorityReportDtos = new List<TasksByPriorityReportDto>(),
                    ActiveTasks = new List<WorkSpaceTaskDashboardDto>()
                };
            }
            return workspaceDashboardDto;
        }
        public async Task<WorkSpaceDashboardDto> GetWorkSpaceDashboardByUserIdAsync(long workspaceId, string userId)
        {
            var workspaceDashboardDto = await appDbContext.WorkSpaces
        .AsNoTracking()
        .Where(w => w.Id == workspaceId)
        .Select(w => new WorkSpaceDashboardDto
        {
            Workspace = new WorkSpaceDashboardSummaryDto
            {
                Id = w.Id,
                Name = w.Name,
            },

            Stats = new WorkSpaceDashboardStatsDto
            {
                TotalTasks = w.Projects
                    .SelectMany(p => p.Tasks).Where(t => t.TaskAssignments.Any(ta => ta.AssignedToId == userId &&
                    ta.IsActive))
                    .Count(),

                CompletedTasks = w.Projects
                    .SelectMany(p => p.Tasks)
                    .Where(t => t.TaskAssignments.Any(ta => ta.AssignedToId == userId &&
                    ta.IsActive))
                    .Count(t => t.TaskStatus == ProjectTaskStatus.Done),

                InProgressTasks = w.Projects
                    .SelectMany(p => p.Tasks)
                    .Where(t => t.TaskAssignments.Any(ta => ta.AssignedToId == userId &&
                    ta.IsActive))
                    .Count(t => t.TaskStatus == ProjectTaskStatus.InProgress),

                TotalProjects = w.Projects.Count(),

                CompletionRate = w.Projects
                    .SelectMany(p => p.Tasks)
                    .Where(t => t.TaskAssignments.Any(ta => ta.AssignedToId == userId &&
                    ta.IsActive))
                    .Count() == 0
                        ? 0
                        : w.Projects
                            .SelectMany(p => p.Tasks)
                            .Count(t => t.TaskStatus == ProjectTaskStatus.Done)
                            / (double)w.Projects
                                .SelectMany(p => p.Tasks)
                                .Count() * 100
            },

            TasksByStatusReportDtos = w.Projects
                .SelectMany(p => p.Tasks)
                .Where(t => t.TaskAssignments.Any(ta => ta.AssignedToId == userId &&
                ta.IsActive))
                .GroupBy(t => t.TaskStatus)
                .Select(g => new TasksByStatusReportDto
                {
                    TaskStatus = g.Key,
                    Count = g.Count()
                })
                .ToList(),

            TasksByPriorityReportDtos = w.Projects
                .SelectMany(p => p.Tasks)
                .Where(t => t.TaskAssignments.Any(ta => ta.AssignedToId == userId &&
                ta.IsActive))
                .GroupBy(t => t.TaskPriority)
                .Select(g => new TasksByPriorityReportDto
                {
                    TaskPriority = g.Key,
                    Count = g.Count()
                })
                .ToList(),

            ActiveTasks = w.Projects
                .SelectMany(p => p.Tasks)
                .Where(t => t.TaskAssignments.Any(ta => 
                ta.AssignedToId == userId && ta.IsActive) &&
                 t.TaskStatus != ProjectTaskStatus.Done)
                .Select(t => new WorkSpaceTaskDashboardDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Status = t.TaskStatus,
                    ProjectName = t.Project.Name,
                    Priority = t.TaskPriority,
                    CreatedAt = t.CreatedAt,
                    DeadLine = t.Deadline
                })
                .ToList()
        })
        .FirstOrDefaultAsync();

            if (workspaceDashboardDto is null)
            {
                return new WorkSpaceDashboardDto
                {
                    Workspace = new WorkSpaceDashboardSummaryDto
                    {
                        Id = workspaceId,
                        Name = string.Empty
                    },
                    Stats = new WorkSpaceDashboardStatsDto
                    {
                        TotalTasks = 0,
                        CompletedTasks = 0,
                        InProgressTasks = 0,
                        TotalProjects = 0,
                        CompletionRate = 0
                    },
                    TasksByStatusReportDtos = new List<TasksByStatusReportDto>(),
                    TasksByPriorityReportDtos = new List<TasksByPriorityReportDto>(),
                    ActiveTasks = new List<WorkSpaceTaskDashboardDto>()
                };
            }
            return workspaceDashboardDto;
        }
    }
}
