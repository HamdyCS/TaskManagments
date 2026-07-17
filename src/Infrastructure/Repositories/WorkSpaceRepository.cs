using Application.Common.Dtos;
using Application.Common.Interfaces.Repositories;
using Domain.Common.Pagination;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class WorkSpaceRepository(AppDbContext context) : GenericRepository<WorkSpace>(context), IWorkSpaceRepository
    {
        public async Task<PaginationResult<WorkSpace>> GetAllUserWorkSpaces(string userId, int pageNumber, int pageSize)
            => await GetAllByFilterAsync(w => w.CreatedById == userId, pageNumber, pageSize,
                w => w.CreatedAt);

        public async Task<string?> GetWorkSpaceNameAsync(long workSpaceId)
            => await context.WorkSpaces.Where(ws => ws.Id == workSpaceId)
               .Select(ws => ws.Name).FirstOrDefaultAsync();
    }
}
