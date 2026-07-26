using Domain.Entities;

namespace Application.Common.Interfaces.Repositories
{
    public interface ITaskAttachmentRepository : IGenericRepository<TaskAttachment>
    {
        Task<List<TaskAttachment>> GetAllByTaskIdAsync(long taskId);
        Task<TaskAttachment?> GetByTaskIdAndIdAsync(long taskId, long attachmentId);
        Task<TaskAttachment?> GetByTaskIdAndNameAsync(long taskId, string name);
    }
}
