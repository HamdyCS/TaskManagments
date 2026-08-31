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
        {
            var query = context.WorkSpaces
                .Include(ws => ws.CreatedBy)
                .Where(ws => ws.WorkSpaceUsers.
            Any(wu => wu.UserId == userId));

            var totalCount = await query.CountAsync();
            var workSpaces = await query.OrderBy(ws => ws.CreatedAt)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginationResult<WorkSpace>(workSpaces, totalCount, pageNumber, pageSize);
        }

        public async Task<string?> GetWorkSpaceNameAsync(long workSpaceId)
            => await context.WorkSpaces.Include(ws => ws.CreatedBy).Where(ws => ws.Id == workSpaceId)
               .Select(ws => ws.Name).FirstOrDefaultAsync();
    }
}
