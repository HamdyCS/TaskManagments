using Application.Common.Dtos.WorkSpaceDashboard;
using Application.Features.Notifications;
using Application.Features.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.WorkSpaceUserDashboard
{
    public class WorkSpaceDashboardDto
    {
        // Workspace context
        public WorkSpaceDashboardSummaryDto Workspace { get; set; }

        // KPI
        public WorkSpaceDashboardStatsDto Stats { get; set; }

        // Charts
        public IEnumerable<TasksByStatusReportDto> TasksByStatusReportDtos { get; set; }

        public IEnumerable<TasksByPriorityReportDto> TasksByPriorityReportDtos { get; set; }

        // Current user's tasks
        public IEnumerable<WorkSpaceTaskDashboardDto> ActiveTasks { get; set; }

        // Un read notifications
        public IEnumerable<NotificationDto> UnReadNotifications { get; set; }

       
    }
}
