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
            => await GetAllByFilterAsync(wu => wu.WorkSpaceId == workSpaceId, pageNumber, pageSize,wu=>wu.CreatedAt);

        public async Task<bool> IsUserHasWorkSpaceRoleAsync(string userId, long workSpaceId,WorkSpaceRole role)
            => await context.WorkSpaceUsers.AnyAsync(wu => wu.UserId == userId && wu.WorkSpaceId == workSpaceId && wu.WorkSpaceRole == role);
    }
}
