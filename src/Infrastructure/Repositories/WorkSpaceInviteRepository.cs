using Application.Common.Interfaces.Repositories;
using Domain.Common.Enums;
using Domain.Common.Pagination;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class WorkSpaceInviteRepository(AppDbContext context) : GenericRepository<WorkSpaceInvite>(context)
        , IWorkSpaceInviteRepository
    {
        public Task<PaginationResult<WorkSpaceInvite>> GetAllWorkSpaceInvitesByInviteToIdAsync(string inviteToId, int pageNumber, int pageSize)
            => GetAllByFilterAsync(wi => wi.InvitedToId == inviteToId
            , pageNumber, pageSize, wi => wi.CreatedAt);

        public Task<PaginationResult<WorkSpaceInvite>> GetAllWorkSpaceInvitesByInviteByIdAsync(string inviteById, int pageNumber, int pageSize)
           => GetAllByFilterAsync(wi => wi.InvitedById == inviteById, pageNumber, pageSize, wi => wi.CreatedAt);

        public async Task<WorkSpaceInvite?> GetWorkSpaceInviteByIdAndInviteByIdAsync(long workSpaceInviteId, string inviteById)
            => await context.WorkSpaceInvites.FirstOrDefaultAsync(wi => wi.Id == workSpaceInviteId
            && wi.InvitedById == inviteById);

        public async Task<WorkSpaceInvite?> GetWorkSpaceInviteByIdAndInviteToIdAsync(long workSpaceInviteId, string inviteById)
            => await context.WorkSpaceInvites.FirstOrDefaultAsync(wi => wi.Id == workSpaceInviteId 
            && wi.InvitedToId == inviteById);

        public async Task<bool> IsUserHasValidWorkSpaceInviteByStatusAsync(string userId, long workSpaceId, WorkSpaceInviteStatus status) => await context.WorkSpaceInvites.AnyAsync(
                wi => wi.WorkSpaceId == workSpaceId && wi.WorkSpaceInviteStatus == status
                && wi.InvitedToId == userId && wi.ExpiresAt > DateTime.UtcNow);
    }
}
