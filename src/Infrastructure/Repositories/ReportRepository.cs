using Application.Common.Dtos;
using Application.Common.Interfaces.Repositories;
using Domain.Common.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class ReportRepository(AppDbContext appDbContext) : IReportRepository
    {
        public async Task<IEnumerable<TasksByStatusReportDto>> GetProjectTasksReportByStatusAsync(long projectId)
        {
            var ProjectTasksReportDtoList = await appDbContext.ProjectTasks
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
            var ProjectTasksReportDtoList = await appDbContext.ProjectTasks
                .Where(t => t.ProjectId == projectId)
                .GroupBy(t => t.TaskPriority)
                .Select(g => new
                 TasksByPriorityReportDto
                { TaskPriority = g.Key, Count = g.Count() })
                .ToListAsync();

            return ProjectTasksReportDtoList;
        }


        public async Task<MemberPerformance> GetMemberPerformanceInWorkSpaceAsync(long workspaceId, string memberId)
        {

            var memberPerformance =  await appDbContext.TaskAssignments
              .Where(ta => ta.AssignedToId == memberId
              && ta.Task.Project.WorkSpaceId == workspaceId)
              .GroupBy(_ => 1)
              .Select(g => new MemberPerformance
              {
                  AssignedCount = g.Count(),
                  InProgressCount = g.Count(ta => ta.Task.TaskStatus == ProjectTaskStatus.InProgress),
                  DoneCount = g.Count(ta => ta.Task.TaskStatus == ProjectTaskStatus.Done)
              }).FirstOrDefaultAsync();

            return memberPerformance ?? new MemberPerformance { AssignedCount = 0, InProgressCount = 0, DoneCount = 0 };
        }

        public async Task<MemberPerformance> GetMemberPerformanceInProjectAsync(long projectId, string memberId)
        {

            var memberPerformance = await appDbContext.TaskAssignments
              .Where(ta => ta.AssignedToId == memberId
              && ta.Task.ProjectId == projectId)
              .GroupBy(_ => 1)
              .Select(g => new MemberPerformance
              {
                  AssignedCount = g.Count(),
                  InProgressCount = g.Count(ta => ta.Task.TaskStatus == ProjectTaskStatus.InProgress),
                  DoneCount = g.Count(ta => ta.Task.TaskStatus == ProjectTaskStatus.Done)
              }).FirstOrDefaultAsync();

            return memberPerformance ?? new MemberPerformance { AssignedCount = 0, InProgressCount = 0, DoneCount = 0 };
        }

        public async Task<WorkSpaceReportDto> GetWorkSpaceReportAsync(long workspaceId)
        {
            var workspaceReport = await appDbContext.WorkSpaces
                .Where(ws => ws.Id == workspaceId)
                .Select(ws => new WorkSpaceReportDto
                {
                    WorkSpaceName = ws.Name,
                    OwnerNames = ws.WorkSpaceUsers.Where(ws=>ws.WorkSpaceRole == WorkSpaceRole.Owner).Select(o => o.User.FirstName + " " + o.User.LastName),
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
            return workspaceReport ?? new WorkSpaceReportDto
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
                TotalDoneTasks = 0
            };
        }
    }
}
