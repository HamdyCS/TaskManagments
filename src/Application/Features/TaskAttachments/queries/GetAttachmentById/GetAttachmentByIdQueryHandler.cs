using Application.Common.Errors;
using Application.Common.Extensions;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using Mapster;

namespace Application.Features.TaskAttachments.Queries.GetAttachmentById
{
    public class GetAttachmentByIdQueryHandler(IFileUrlService fileUrlService,
        IUnitOfWork unitOfWork,
        ILogger<GetAttachmentByIdQueryHandler> logger) : IRequestHandler<GetAttachmentByIdQuery, ErrorOr<TaskAttachmentDto>>
    {
        public async Task<ErrorOr<TaskAttachmentDto>> Handle(GetAttachmentByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetAttachmentById with id {AttachmentId} for task {TaskId}", request.AttachmentId, request.TaskId);

            // Verify task exists and belongs to project
            var task = await unitOfWork.TaskRepository.GetByIdAndWorkSpaceIdAndProjectIdAsync(request.TaskId,request.WorkSpaceId
                , request.ProjectId);
            if (task is null)
                return TaskAttachmentErrors.TaskNotFound(request.TaskId);

            var attachment = await unitOfWork.TaskAttachmentRepository.GetByTaskIdAndIdAsync(request.TaskId, request.AttachmentId);
            if (attachment is null)
                return TaskAttachmentErrors.NotFound(request.AttachmentId);

            logger.LogInformation("GetAttachmentById with id {AttachmentId} for task {TaskId} successfully", request.AttachmentId, request.TaskId);
            return attachment.ToTaskAttachmentDto(fileUrlService);
        }
    }
}
