using Domain.Common.Enums;
using Domain.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Repositories
{
    public interface IWorkSpaceInviteRepository : IGenericRepository<WorkSpaceInvite>
    {
        Task<WorkSpaceInvite?> GetWorkSpaceInviteByIdAndInviteByIdAsync(long workSpaceInviteId, string inviteById);

        Task<WorkSpaceInvite?> GetWorkSpaceInviteByIdAndInviteToIdAsync(long workSpaceInviteId, string inviteByTo);

        Task<bool> IsUserHasValidWorkSpaceInviteByStatusAsync(string userId,long workSpaceId, WorkSpaceInviteStatus status);

        Task<PaginationResult<WorkSpaceInvite>> GetAllWorkSpaceInvitesByInviteToIdAsync(string inviteToId, int pageNumber, int pageSize);
        Task<PaginationResult<WorkSpaceInvite>> GetAllWorkSpaceInvitesByInviteByIdAsync(string inviteById, int pageNumber, int pageSize);
    }
}
