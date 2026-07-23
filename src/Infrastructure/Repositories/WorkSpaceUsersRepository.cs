using Application.Common.Interfaces.Repositories;
using Domain.Common.Enums;
using Domain.Common.Pagination;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Infrastructure.Repositories
{
    public class WorkSpaceUsersRepository(AppDbContext context) : GenericRepository<WorkSpaceUser>(context),
        IWorkSpaceUserRepository
    {
        public async Task<bool> IsUserExistInWorkSpaceAsync(string userId, long workSpaceId) =>
              await context.WorkSpaceUsers.AnyAsync(wu => wu.UserId == userId && wu.WorkSpaceId == workSpaceId);

        public async Task<WorkSpaceUser?> GetWorkSpaceUserAsync(string userId, long workSpaceId)
            => await context.WorkSpaceUsers.
            FirstOrDefaultAsync(wu => wu.UserId == userId && wu.WorkSpaceId == workSpaceId);

        public async Task<PaginationResult<WorkSpaceUser>> GetWorkSpaceUsersAsync(long workSpaceId, int pageNumber, int pageSize)
        {
            var query = context.WorkSpaceUsers.Include(wu => wu.User)
                .Where(wu => wu.WorkSpaceId == workSpaceId).OrderBy(wu => wu.CreatedAt);

            var totalCount = await query.CountAsync();
            var workSpaceUsers = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginationResult<WorkSpaceUser>(workSpaceUsers, totalCount, pageNumber, pageSize);
        }

        public async Task<bool> IsUserHasWorkSpaceRoleAsync(string userId, long workSpaceId,WorkSpaceRole role)
            => await context.WorkSpaceUsers.AnyAsync(wu => wu.UserId == userId && wu.WorkSpaceId == workSpaceId && wu.WorkSpaceRole == role);
    }
}
