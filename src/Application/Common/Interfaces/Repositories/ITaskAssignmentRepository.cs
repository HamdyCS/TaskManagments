using Domain.Entities;

namespace Application.Common.Interfaces.Repositories
{
    public interface ITaskAssignmentRepository : IGenericRepository<TaskAssignment>
    {
        void Add(TaskAssignment entity);
        void AddRange(IEnumerable<TaskAssignment> entities);
        void Delete(TaskAssignment entity);
        Task<TaskAssignment?> GetByTaskIdAndAssignedToIdAsync(long taskId, string userId);
        Task<List<TaskAssignment>> GetActiveAssignmentsByTaskIdAsync(long taskId);
        Task<bool> HasActiveAssignmentAsync(long taskId, string userId);
        Task<TaskAssignment> GetActiveAssignmentByTaskIdAsync(long taskId);
    }
}
