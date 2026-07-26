using Domain.Common.Enums;
using Domain.Common.Pagination;
using Domain.Entities;

namespace Application.Common.Interfaces.Repositories
{
    public interface ITaskRepository : IGenericRepository<ProjectTask>
    {
     
        Task<ProjectTask?> GetByIdAndProjectIdAsync(long id, long projectId);
        Task<PaginationResult<ProjectTask>> GetAllByProjectIdAsync(long projectId, int pageNumber, int pageSize);
        Task<PaginationResult<ProjectTask>> GetAllFilteredAsync(long projectId, int pageNumber, int pageSize, ProjectTaskStatus? status, TaskPriority? priority, string? searchTerm, string? sortBy, string? sortOrder);
        Task<PaginationResult<ProjectTask>> GetByProjectIdAndUserIdAsync(long projectId, string userId, int pageNumber, int pageSize);
        Task<PaginationResult<ProjectTask>> GetByProjectIdAndUserIdFilteredAsync(long projectId, string userId, int pageNumber, int pageSize, ProjectTaskStatus? status, TaskPriority? priority, string? searchTerm, string? sortBy, string? sortOrder);
        Task<bool> IsTaskNameUniqueInProjectAsync(long projectId, string name, long? excludeTaskId = null);
        Task<ProjectTask?> GetByIdAndWorkSpaceIdAndProjectIdAsync(long id, long workSpaceId, long projectId);
        Task<ProjectTask?> GetByIdAndWorkSpaceIdAndProjectIdAndAssignedToIdAsync(long id, long workSpaceId, long projectId,string assignedToId);
    }
}
