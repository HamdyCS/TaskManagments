using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.AdminDashboard
{
    public class AdminDashboardDto
    {
        public int TotalUsersCount { get; set; }

        public int TotalAdminsCount { get; set; }

        public int TotalUsersInLast30DaysCount { get; set; }

        public int TotalWorkspacesCount { get; set; }

        public int TotalWorkspacesInLast30DaysCount { get; set; }

        public int TotalProjectsCount { get; set; }

        public int TotalProjectsInLast30DaysCount { get; set; }

        public int TotalTasksCount { get; set; }

        public int TotalTasksInLast30DaysCount { get; set; }

        public TasksOverviewDto TasksOverviewDto { get; set; }

    }
}
