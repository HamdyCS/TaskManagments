using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TaskAssignmentRepository : GenericRepository<TaskAssignment>, ITaskAssignmentRepository
    {
        public TaskAssignmentRepository(AppDbContext context) : base(context)
        {
        }

        
        public async Task<TaskAssignment?> GetByTaskIdAndAssignedToIdAsync(long taskId, string userId)
        {
            return await context.Set<TaskAssignment>()
                .FirstOrDefaultAsync(ta => ta.TaskId == taskId && ta.AssignedToId == userId && ta.IsActive);
        }

        public async Task<List<TaskAssignment>> GetActiveAssignmentsByTaskIdAsync(long taskId)
        {
            return await context.Set<TaskAssignment>()
                .Where(ta => ta.TaskId == taskId && ta.IsActive)
                .ToListAsync();
        }

        public async Task<bool> HasActiveAssignmentAsync(long taskId, string userId)
        {
            return await context.Set<TaskAssignment>()
                .AnyAsync(ta => ta.TaskId == taskId && ta.AssignedToId == userId && ta.IsActive);
        }

        public Task<TaskAssignment> GetActiveAssignmentByTaskIdAsync(long taskId)
             => GetByFilterAsync(ta => ta.TaskId == taskId && ta.IsActive);
    }
}
