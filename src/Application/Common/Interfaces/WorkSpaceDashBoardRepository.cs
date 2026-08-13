using Application.Common.Dtos.WorkSpaceUserDashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces
{
    public interface IWorkSpaceDashboardRepository
    {
        Task<WorkSpaceDashboardDto> GetWorkSpaceDashboardByUserIdAsync(long workspaceId, string userId);
        Task<WorkSpaceDashboardDto> GetWorkSpaceDashboardAsync(long workspaceId);
    }
}
