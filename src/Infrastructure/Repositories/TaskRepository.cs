using Application.Common.Interfaces.Repositories;
using Domain.Common.Enums;
using Domain.Common.Pagination;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<ProjectTask>, ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PaginationResult<ProjectTask>> GetAllByProjectIdAsync(long projectId, int pageNumber, int pageSize)
        {
            var query = context.Set<ProjectTask>()
                .Include(t => t.TaskAssignments).Where(t => t.ProjectId == projectId)
                .Include(t => t.TaskAttachments);
            var totalCount = await query.CountAsync();
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResult<ProjectTask>(data, totalCount, pageNumber, pageSize);
        }

        public async Task<PaginationResult<ProjectTask>> GetAllFilteredAsync(long projectId, int pageNumber, int pageSize, ProjectTaskStatus? status, TaskPriority? priority, string? searchTerm, string? sortBy, string? sortOrder)
        {
            var query = context.Set<ProjectTask>().Include(t => t.TaskAssignments)
                .Include(t => t.TaskAttachments)
                .Where(t => t.ProjectId == projectId);

            if (status.HasValue)
                query = query.Where(t => t.TaskStatus == status.Value);

            if (priority.HasValue)
                query = query.Where(t => t.TaskPriority == priority.Value);

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(t => t.Name.Contains(searchTerm) || (t.Description != null && t.Description.Contains(searchTerm)));

            var totalCount = await query.CountAsync();

            bool isDescending = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);

            query = sortBy?.ToLower() switch
            {
                "name" => isDescending ? query.OrderByDescending(t => t.Name) : query.OrderBy(t => t.Name),
                "status" => isDescending ? query.OrderByDescending(t => t.TaskStatus) : query.OrderBy(t => t.TaskStatus),
                "priority" => isDescending ? query.OrderByDescending(t => t.TaskPriority) : query.OrderBy(t => t.TaskPriority),
                "deadline" => isDescending ? query.OrderByDescending(t => t.Deadline) : query.OrderBy(t => t.Deadline),
                _ => isDescending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt)
            };

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResult<ProjectTask>(data, totalCount, pageNumber, pageSize);
        }

        public async Task<PaginationResult<ProjectTask>> GetByProjectIdAndUserIdAsync(long projectId, string userId, int pageNumber, int pageSize)
        {
            var query = context.Set<ProjectTask>().Include(t=>t.TaskAssignments)
                .Include(t => t.TaskAttachments)
                .Where(t => t.ProjectId == projectId && t.TaskAssignments.Any(a => a.AssignedToId == userId && a.IsActive));

            var totalCount = await query.CountAsync();
            var data = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResult<ProjectTask>(data, totalCount, pageNumber, pageSize);
        }

        public async Task<PaginationResult<ProjectTask>> GetByProjectIdAndUserIdFilteredAsync(long projectId, string userId, int pageNumber, int pageSize, ProjectTaskStatus? status, TaskPriority? priority, string? searchTerm, string? sortBy, string? sortOrder)
        {
            var query = context.Set<ProjectTask>().Include(t => t.TaskAssignments)
                .Include(t => t.TaskAttachments)
                .Where(t => t.ProjectId == projectId && t.TaskAssignments
                .Any(a => a.AssignedToId == userId && a.IsActive));

            if (status.HasValue)
                query = query.Where(t => t.TaskStatus == status.Value);

            if (priority.HasValue)
                query = query.Where(t => t.TaskPriority == priority.Value);

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(t => t.Name.Contains(searchTerm) || (t.Description != null && t.Description.Contains(searchTerm)));

            var totalCount = await query.CountAsync();

            bool isDescending = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);

            query = sortBy?.ToLower() switch
            {
                "name" => isDescending ? query.OrderByDescending(t => t.Name) : query.OrderBy(t => t.Name),
                "status" => isDescending ? query.OrderByDescending(t => t.TaskStatus) : query.OrderBy(t => t.TaskStatus),
                "priority" => isDescending ? query.OrderByDescending(t => t.TaskPriority) : query.OrderBy(t => t.TaskPriority),
                "deadline" => isDescending ? query.OrderByDescending(t => t.Deadline) : query.OrderBy(t => t.Deadline),
                _ => isDescending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt)
            };

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResult<ProjectTask>(data, totalCount, pageNumber, pageSize);
        }

        public async Task<bool> IsTaskNameUniqueInProjectAsync(long projectId, string name, long? excludeTaskId = null)
        {
            var query = context.Set<ProjectTask>()
                .Where(t => t.ProjectId == projectId && t.Name == name && !t.IsDeleted);

            if (excludeTaskId.HasValue)
            {
                query = query.Where(t => t.Id != excludeTaskId.Value);
            }

            return !await query.AnyAsync();
        }

        public Task<ProjectTask?> GetByIdAndProjectIdAsync(long id, long projectId)
            => context.ProjectTasks.Include(t => t.TaskAssignments).Include(t => t.TaskAttachments)
            .FirstOrDefaultAsync(t => t.Id == id && t.ProjectId == projectId);

        public Task<ProjectTask?> GetByIdAndWorkSpaceIdAndProjectIdAsync(long id,long workSpaceId ,long projectId)
           => context.ProjectTasks.Include(t => t.TaskAssignments).Include(t=>t.TaskAttachments)
           .FirstOrDefaultAsync(t => t.Id == id && t.Project.WorkSpaceId == workSpaceId && t.ProjectId == projectId);

    }
}
