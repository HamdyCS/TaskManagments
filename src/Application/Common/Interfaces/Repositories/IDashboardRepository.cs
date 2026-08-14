using Application.Common.Dtos.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<DashboardDto> GetWorkSpaceDashboardByUserIdAsync(long workspaceId, string userId);
        Task<DashboardDto> GetWorkSpaceDashboardAsync(long workspaceId);
    }
}
