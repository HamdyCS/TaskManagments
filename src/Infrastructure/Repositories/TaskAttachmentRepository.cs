using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TaskAttachmentRepository : GenericRepository<TaskAttachment>, ITaskAttachmentRepository
    {
        public TaskAttachmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<TaskAttachment>> GetAllByTaskIdAsync(long taskId)
        {
            return await context.Set<TaskAttachment>()
                .Where(ta => ta.TaskId == taskId)
                .OrderByDescending(ta => ta.CreatedAt)
                .ToListAsync();
        }

        public async Task<TaskAttachment?> GetByTaskIdAndIdAsync(long taskId, long attachmentId)
        {
            return await context.Set<TaskAttachment>()
                .FirstOrDefaultAsync(ta => ta.TaskId == taskId && ta.Id == attachmentId);
        }

        public async Task<TaskAttachment?> GetByTaskIdAndNameAsync(long taskId, string name)
        {
            return await context.Set<TaskAttachment>()
                .Where(ta => ta.TaskId == taskId && ta.Name == name)
                .OrderByDescending(ta => ta.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
