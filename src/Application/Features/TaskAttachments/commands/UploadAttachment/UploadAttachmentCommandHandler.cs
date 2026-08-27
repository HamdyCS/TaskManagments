using Application.Common.Errors;
using Application.Common.Extensions;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Entities;
using ErrorOr;
using Mapster;

namespace Application.Features.TaskAttachments.Commands.UploadAttachment
{
    public class UploadAttachmentCommandHandler(
        IFileUrlService fileUrlService,
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ILogger<UploadAttachmentCommandHandler> logger) : IRequestHandler<UploadAttachmentCommand, ErrorOr<TaskAttachmentDto>>
    {
        public async Task<ErrorOr<TaskAttachmentDto>> Handle(UploadAttachmentCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting UploadAttachment for task {TaskId} by user with id {UserId}", request.TaskId, request.UserId);


            var file = request.UploadAttachmentDto.File;
            using var stream =  request.UploadAttachmentDto.File.OpenReadStream();
            // Validate file

            if (stream.Length == 0)
                return TaskAttachmentErrors.EmptyFile();

            if (stream.Length > TaskAttachmentConstants.MaxFileSize)
                return TaskAttachmentErrors.FileTooLarge();

            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(extension) || !TaskAttachmentConstants.AllowedExtensions.Contains(extension))
                return TaskAttachmentErrors.InvalidExtension();

            //check mime type for security
            if (!TaskAttachmentConstants.AllowedMimeTypes.Contains(file.ContentType))
                return TaskAttachmentErrors.InvalidMimeType();

          
            // Verify task exists and belongs to project
            var task = await unitOfWork.TaskRepository.GetByIdAndWorkSpaceIdAndProjectIdAsync(request.TaskId,request.WorkSpaceId, request.ProjectId);
            if (task is null)
                return TaskAttachmentErrors.TaskNotFound(request.TaskId);

            // Save file to disk
            string storageKey;
            try
            {
                storageKey = await fileStorageService.SaveFileAsync(stream, extension, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to save file for task {TaskId} by user with id {UserId}", request.TaskId, request.UserId);
                return TaskAttachmentErrors.FileSaveFailed();
            }

            // Save metadata to database
            var attachment = new TaskAttachment
            {
                Name = Path.GetFileNameWithoutExtension(file.FileName),
                StorageKey = storageKey,
                TaskId = request.TaskId,
                CreatedById = request.UserId,
                CreatedAt = DateTime.UtcNow,
                Size = stream.Length,
                ContentType = file.ContentType,
            };

            unitOfWork.TaskAttachmentRepository.Add(attachment);

            var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!isSaved)
            {
                // Best-effort cleanup: delete the orphaned file
                try
                {
                    await fileStorageService.DeleteFileAsync(storageKey, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to cleanup orphaned file {Url} after database save failure", storageKey);
                }

                logger.LogWarning("Failed to save attachment metadata for task {TaskId} by user with id {UserId}", request.TaskId, request.UserId);
                return TaskAttachmentErrors.DatabaseSaveFailed();
            }

            logger.LogInformation("UploadAttachment for task {TaskId} by user with id {UserId} successfully", request.TaskId, request.UserId);
            return attachment.ToTaskAttachmentDto(fileUrlService);
        }
    }
}
