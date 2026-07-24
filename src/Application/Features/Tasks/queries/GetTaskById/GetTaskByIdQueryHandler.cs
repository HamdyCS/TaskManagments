using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using ErrorOr;
using Mapster;
using MediatR;

namespace Application.Features.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetTaskByIdQueryHandler> logger) : IRequestHandler<GetTaskByIdQuery, ErrorOr<TaskDto>>
    {
        public async Task<ErrorOr<TaskDto>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetTaskById with id {TaskId} in project {ProjectId}", request.TaskId, request.ProjectId);

            // Verify project belongs to workspace
            var project = await unitOfWork.ProjectRepository.GetByIdAndWorkSpaceIdAsync(request.ProjectId, request.WorkSpaceId);
            if (project is null)
                return TaskErrors.ProjectNotInWorkspace(request.ProjectId, request.WorkSpaceId);

            var task = await unitOfWork.TaskRepository.GetByIdAsync(request.TaskId);
            if (task is null || task.ProjectId != request.ProjectId)
                return TaskErrors.TaskNotFound(request.TaskId);

            logger.LogInformation("GetTaskById with id {TaskId} in project {ProjectId} completed successfully", request.TaskId, request.ProjectId);

            return task.Adapt<TaskDto>();
        }
    }
}
