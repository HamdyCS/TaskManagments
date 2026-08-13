using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.WorkSpaceUserDashboard
{
    public class WorkSpaceDashboardStatsDto
    {
        public int TotalProjects { get; set; }

        public int TotalTasks { get; set; }

        public int InProgressTasks { get; set; }

        public int CompletedTasks { get; set; }

        public double CompletionRate { get; set; }
    }
}
