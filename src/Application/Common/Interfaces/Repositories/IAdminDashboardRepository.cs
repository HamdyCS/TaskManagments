using Application.Common.Dtos.AdminDashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Repositories
{
    public interface IAdminDashboardRepository
    {
        public Task<AdminDashboardDto> GetAdminDashboardDataAsync();
    }
}
