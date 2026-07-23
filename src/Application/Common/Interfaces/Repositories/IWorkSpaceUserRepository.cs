using Domain.Common.Enums;
using Domain.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Repositories
{
    public interface IWorkSpaceUserRepository : IGenericRepository<WorkSpaceUser>
    {
        Task<WorkSpaceUser?> GetWorkSpaceUserAsync(string userId, long workSpaceId);
        Task<PaginationResult<WorkSpaceUser>> GetWorkSpaceUsersAsync(long workSpaceId, int pageNumber, int pageSize);
        Task<bool> IsUserExistInWorkSpaceAsync(string userId, long workSpaceId);
        Task<bool> IsUserHasWorkSpaceRoleAsync(string userId, long workSpaceId, WorkSpaceRole role);
    }
}
