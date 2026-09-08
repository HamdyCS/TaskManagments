using Application.Common.Interfaces.Repositories;
using Domain.Common.Pagination;
using Domain.Entities;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class RecentActivityRepository(AppDbContext context) : GenericRepository<RecentActivity>(context), 
        IRecentActivityRepository
    {
      public override async Task<PaginationResult<RecentActivity>> GetAllAsync(int pageNumber,int pageSize)
            => await GetAllByFilterAsync(x=>true,pageNumber,pageSize, x => x.CreatedAt, false);
    }
}
