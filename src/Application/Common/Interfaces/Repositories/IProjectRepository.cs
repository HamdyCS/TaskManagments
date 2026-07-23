using Domain.Common.Pagination;
using Domain.Entities;

namespace Application.Common.Interfaces.Repositories
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<bool> IsProjectNameUniqueInWorkspaceAsync(long workSpaceId, string name, long? excludeProjectId = null);
        Task<PaginationResult<Project>> GetAllByWorkSpaceIdAsync(long workSpaceId, int pageNumber, int pageSize);
        Task<Project?> GetByIdAndWorkSpaceIdAsync(long projectId, long workSpaceId);
    }
}
