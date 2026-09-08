using Application.Common.Dtos.AdminDashboard;
using Application.Common.Interfaces.Repositories;
using Domain.Common.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
        public class AdminDashboardRepository(AppDbContext context) : IAdminDashboardRepository
        {
        public async Task<AdminDashboardDto> GetAdminDashboardDataAsync()
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            return await context.Users
                .Select(_ => new AdminDashboardDto
                {
                    TotalUsersCount = context.Users
                        .Count(u => u.RoleId == (int)Role.User),

                    TotalAdminsCount = context.Users
                        .Count(u => u.RoleId == (int)Role.Admin),

                    TotalUsersInLast30DaysCount = context.Users
                        .Count(u =>
                            u.RoleId == (int)Role.User &&
                            u.CreatedAt >= thirtyDaysAgo),

                    TotalWorkspacesCount = context.WorkSpaces.Count(),

                    TotalWorkspacesInLast30DaysCount = context.WorkSpaces
                        .Count(w => w.CreatedAt >= thirtyDaysAgo),

                    TotalProjectsCount = context.Projects.Count(),

                    TotalProjectsInLast30DaysCount = context.Projects
                        .Count(p => p.CreatedAt >= thirtyDaysAgo),

                    TotalTasksCount = context.ProjectTasks.Count(),

                    TotalTasksInLast30DaysCount = context.ProjectTasks
                        .Count(t => t.CreatedAt >= thirtyDaysAgo),

                    TasksOverviewDto = new TasksOverviewDto
                    {
                        BacklogCount = context.ProjectTasks
                            .Count(t => t.TaskStatus == ProjectTaskStatus.Backlog),

                        TodoCount = context.ProjectTasks
                            .Count(t => t.TaskStatus == ProjectTaskStatus.Todo),

                        InProgressCount = context.ProjectTasks
                            .Count(t => t.TaskStatus == ProjectTaskStatus.InProgress),

                        ReviewCount = context.ProjectTasks
                            .Count(t => t.TaskStatus == ProjectTaskStatus.Review),

                        DoneCount = context.ProjectTasks
                            .Count(t => t.TaskStatus == ProjectTaskStatus.Done)
                    }
                })
                .FirstOrDefaultAsync()
                ?? new AdminDashboardDto();
        }
    }
    }

