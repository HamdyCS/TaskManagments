using Domain.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Services
{
    public interface IRecentActivityService
    {
        Task<bool> AddAsync(RecentActivity recentActivity);

        Task<PaginationResult<RecentActivity>> GetAllAsync(int pageNumber, int pageSize);
    }
}
