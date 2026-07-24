using Application.Common.Interfaces.Repositories;
using Domain.Common.Enums;
using Domain.Common.Pagination;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> IsProjectNameUniqueInWorkspaceAsync(long workSpaceId, string name, long? excludeProjectId = null)
        {
            var query = context.Set<Project>()
                .Where(p => p.WorkSpaceId == workSpaceId && p.Name == name && !p.IsDeleted);

            if (excludeProjectId.HasValue)
            {
                query = query.Where(p => p.Id != excludeProjectId.Value);
            }

            var isExist = await query.AnyAsync();
            return !isExist;
        }

        public async Task<PaginationResult<Project>> GetAllByWorkSpaceIdAsync(long workSpaceId, int pageNumber, int pageSize)
            => await GetAllByFilterAsync(p => p.WorkSpaceId == workSpaceId, pageNumber, pageSize, p => p.CreatedAt);

        public async Task<Project?> GetByIdAndWorkSpaceIdAsync(long projectId, long workSpaceId)
            => await GetByFilterAsync(p => p.Id == projectId && p.WorkSpaceId == workSpaceId);

        public async Task<int> UpdateStatusAsync(long projectId, long workSpaceId, string userId, ProjectStatus status)
        {
            return await context.Projects
                .Where(p => p.Id == projectId && p.WorkSpaceId == workSpaceId && !p.IsDeleted)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.Status, status)
                    .SetProperty(p => p.LastUpdatedAt, DateTime.UtcNow)
                    .SetProperty(p => p.LastUpdatedById, userId));

        }

      
    }
}
