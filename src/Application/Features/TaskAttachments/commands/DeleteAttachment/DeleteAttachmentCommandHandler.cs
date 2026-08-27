using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;

namespace Application.Features.TaskAttachments.Commands.DeleteAttachment
{
    public class DeleteAttachmentCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ILogger<DeleteAttachmentCommandHandler> logger) : IRequestHandler<DeleteAttachmentCommand, ErrorOr<Deleted>>
    {
        public async Task<ErrorOr<Deleted>> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting DeleteAttachment with id {AttachmentId} for task {TaskId} by user with id {UserId}", request.AttachmentId, request.TaskId, request.UserId);
      
            // Verify task exists and belongs to project
            var task = await unitOfWork.TaskRepository.GetByIdAndWorkSpaceIdAndProjectIdAsync(request.TaskId,request.WorkSpaceId, request.ProjectId);
            if (task is null)
                return TaskAttachmentErrors.TaskNotFound(request.TaskId);

            var attachment = await unitOfWork.TaskAttachmentRepository.GetByTaskIdAndIdAsync(request.TaskId, request.AttachmentId);
            if (attachment is null)
                return TaskAttachmentErrors.NotFound(request.AttachmentId);

            // Delete physical file first
            try
            {
                await fileStorageService.DeleteFileAsync(attachment.StorageKey, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to delete file {Url} for attachment {AttachmentId}", attachment.StorageKey, request.AttachmentId);
                return TaskAttachmentErrors.FileDeleteFailed();
            }

            // Delete database record
            unitOfWork.TaskAttachmentRepository.Delete(attachment);

            var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!isSaved)
            {
                logger.LogWarning("Failed to delete attachment {AttachmentId} from database", request.AttachmentId);
                return TaskAttachmentErrors.DatabaseSaveFailed();
            }

            logger.LogInformation("DeleteAttachment with id {AttachmentId} for task {TaskId} by user with id {UserId} successfully", request.AttachmentId, request.TaskId, request.UserId);
            return Result.Deleted;
        }
    }
}
