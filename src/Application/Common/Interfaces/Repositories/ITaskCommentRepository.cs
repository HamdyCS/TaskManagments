using Domain.Common.Pagination;
using Domain.Entities;

namespace Application.Common.Interfaces.Repositories
{
    public interface ITaskCommentRepository : IGenericRepository<TaskComment>
    {
        Task<PaginationResult<TaskComment>> GetAllByTaskIdAsync(long taskId, int pageNumber, int pageSize);
        Task<IEnumerable<TaskComment>> GetAllWithoutPaginationByTaskIdAsync(long taskId);
        Task<TaskComment?> GetByTaskIdAndCommentedByIdAndIdAsync(long taskId, string commentedById, long commentId);
        Task<TaskComment?> GetByTaskIdAndIdAsync(long taskId, long commentId);
    }
}
