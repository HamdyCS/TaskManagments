using Application.Common.Errors;
using Application.Common.Extensions;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Features.TaskAttachments.queries.DownloadAttachmentById;
using ErrorOr;
using Mapster;

namespace Application.Features.TaskAttachments.Queries.DownloadAttachmentById
{
    public class DownloadAttachmentByIdQueryHandler(IFileUrlService fileUrlService,
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ILogger<DownloadAttachmentByIdQueryHandler> logger) : IRequestHandler<DownloadAttachmentByIdQuery, ErrorOr<DownloadAttachmentResultDto>>
    {
        public async Task<ErrorOr<DownloadAttachmentResultDto>> Handle(DownloadAttachmentByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting download attachment with id {AttachmentId} for task {TaskId}", request.AttachmentId, request.TaskId);

            logger.LogInformation("Getting task with id {TaskId} by workspace id {WorkSpaceId} and project id {ProjectId}", request.TaskId, request.WorkSpaceId, request.ProjectId);
            // Verify task exists and belongs to project
            var task = await unitOfWork.TaskRepository.GetByIdAndWorkSpaceIdAndProjectIdAsync(request.TaskId,request.WorkSpaceId
                , request.ProjectId);
            if (task is null)
                return TaskAttachmentErrors.TaskNotFound(request.TaskId);

            logger.LogInformation("Getting attachment with id {AttachmentId} for task {TaskId}", request.AttachmentId, request.TaskId);
            var attachment = await unitOfWork.TaskAttachmentRepository.GetByTaskIdAndIdAsync(request.TaskId, request.AttachmentId);
            if (attachment is null)
                return TaskAttachmentErrors.NotFound(request.AttachmentId);

            //get file extension from storageKey
            var fileExtension = Path.GetExtension(attachment.StorageKey);
            var nameWithExtension = $"{attachment.Name}{fileExtension}";

            var stream = await fileStorageService.GetTaskAttachmentFileAsync(attachment.StorageKey, cancellationToken);
            logger.LogInformation("Download attachment with id {AttachmentId} for task {TaskId} successfully", request.AttachmentId, request.TaskId);
            return new DownloadAttachmentResultDto(stream, nameWithExtension, attachment.ContentType);
        }
    }
}
