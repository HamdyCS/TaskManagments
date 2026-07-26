using Application.Common.Errors;
using Application.Common.Extensions;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using Mapster;

namespace Application.Features.TaskAttachments.Queries.GetAttachmentByName
{
    public class GetAttachmentByNameQueryHandler(
        IFileUrlService fileUrlService,
        IUnitOfWork unitOfWork,
        ILogger<GetAttachmentByNameQueryHandler> logger) : IRequestHandler<GetAttachmentByNameQuery, ErrorOr<TaskAttachmentDto>>
    {
        public async Task<ErrorOr<TaskAttachmentDto>> Handle(GetAttachmentByNameQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetAttachmentByName with name {Name} for task {TaskId}", request.Name, request.TaskId);

        
            // Verify task exists and belongs to project
            var task = await unitOfWork.TaskRepository.GetByIdAndWorkSpaceIdAndProjectIdAsync(request.TaskId,request.WorkSpaceId, request.ProjectId);
            if (task is null)
                return TaskAttachmentErrors.TaskNotFound(request.TaskId);

            var attachment = await unitOfWork.TaskAttachmentRepository.GetByTaskIdAndNameAsync(request.TaskId, request.Name);
            if (attachment is null)
                return TaskAttachmentErrors.NotFoundByName(request.Name);

            logger.LogInformation("GetAttachmentByName with name {Name} for task {TaskId} successfully", request.Name, request.TaskId);
            return attachment.ToTaskAttachmentDto(fileUrlService);
        }
    }
}
