using Application.Common.Interfaces.Repositories;
using Domain.Common.Pagination;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TaskCommentRepository : GenericRepository<TaskComment>, ITaskCommentRepository
    {
        public TaskCommentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TaskComment>> GetAllWithoutPaginationByTaskIdAsync(long taskId)
        {
            return await context.Set<TaskComment>()
                .Where(tc => tc.TaskId == taskId)
                .OrderByDescending(tc => tc.CreatedAt)
                .ToListAsync();
        }

        public async Task<PaginationResult<TaskComment>> GetAllByTaskIdAsync(long taskId, int pageNumber, int pageSize)
        {
            var query = context.Set<TaskComment>()
                .Where(tc => tc.TaskId == taskId)
                .Include(tc => tc.CommentBy);

            var totalCount = await query.CountAsync();
            var data = await query
                .OrderByDescending(tc => tc.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResult<TaskComment>(data, totalCount, pageNumber, pageSize);
        }

        public async Task<TaskComment?> GetByTaskIdAndIdAsync(long taskId, long commentId)
            => await GetByFilterAsync(tc => tc.TaskId == taskId && tc.Id == commentId);

        public async Task<TaskComment?> GetByTaskIdAndCommentedByIdAndIdAsync(long taskId, string commentedById, long commentId)
            => await GetByFilterAsync(tc => tc.TaskId == taskId && tc.CommentById == commentedById 
            && tc.Id == commentId);

    }
}
