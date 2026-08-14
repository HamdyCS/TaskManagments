using Application.Features.Notifications;
using Application.Features.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Dashboard
{
    public class DashboardDto
    {
        // Workspace context
        public DashboardSummaryDto Workspace { get; set; }

        // KPI
        public DashboardStatsDto Stats { get; set; }

        // Charts
        public IEnumerable<TasksByStatusReportDto> TasksByStatusReportDtos { get; set; }

        public IEnumerable<TasksByPriorityReportDto> TasksByPriorityReportDtos { get; set; }

        // Current user's tasks
        public IEnumerable<DashboardTasksDto> ActiveTasks { get; set; }

        // Un read notifications
        public IEnumerable<NotificationDto> UnReadNotifications { get; set; }

       
    }
}
