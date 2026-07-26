using Application.Common.Errors;
using Application.Common.Extensions;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using Mapster;

namespace Application.Features.TaskAttachments.Queries.GetAttachmentsByTaskId
{
    public class GetAttachmentsByTaskIdQueryHandler(
        IFileUrlService fileUrlService,
        IUnitOfWork unitOfWork,
        ILogger<GetAttachmentsByTaskIdQueryHandler> logger) : IRequestHandler<GetAttachmentsByTaskIdQuery, ErrorOr<List<TaskAttachmentDto>>>
    {
        public async Task<ErrorOr<List<TaskAttachmentDto>>> Handle(GetAttachmentsByTaskIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetAttachmentsByTaskId for task {TaskId}", request.TaskId);
     
            // Verify task exists and belongs to project
            var task = await unitOfWork.TaskRepository.GetByIdAndWorkSpaceIdAndProjectIdAsync(request.TaskId,request.WorkSpaceId, request.ProjectId);
            if (task is null)
                return TaskAttachmentErrors.TaskNotFound(request.TaskId);

            var attachments = await unitOfWork.TaskAttachmentRepository.GetAllByTaskIdAsync(request.TaskId);

            logger.LogInformation("GetAttachmentsByTaskId for task {TaskId} successfully", request.TaskId);
            return attachments.ToTaskAttachmentDtoList(fileUrlService);
        }
    }
}
