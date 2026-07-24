using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using ErrorOr;
using Mapster;
using MediatR;

namespace Application.Features.Tasks.Queries.GetMyTaskById
{
    public class GetMyTaskByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetMyTaskByIdQueryHandler> logger) : IRequestHandler<GetMyTaskByIdQuery, ErrorOr<TaskDto>>
    {
        public async Task<ErrorOr<TaskDto>> Handle(GetMyTaskByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetMyTaskById with id {TaskId} in project {ProjectId} for user {AssignedToId}", request.TaskId, request.ProjectId, request.AssignedToId);

            var project = await unitOfWork.ProjectRepository.GetByIdAndWorkSpaceIdAsync(request.ProjectId, request.WorkSpaceId);
            if (project is null)
                return TaskErrors.ProjectNotInWorkspace(request.ProjectId, request.WorkSpaceId);

            var task = await unitOfWork.TaskRepository.GetByIdAndProjectIdAsync(request.TaskId, request.ProjectId);
            if (task is null)
                return TaskErrors.TaskNotFound(request.TaskId);

            var isAssigned = await unitOfWork.TaskAssignmentRepository.HasActiveAssignmentAsync(request.TaskId, request.AssignedToId);
            if (!isAssigned)
                return TaskErrors.NotAssignedToTask(request.TaskId, request.AssignedToId);

            logger.LogInformation("GetMyTaskById with id {TaskId} in project {ProjectId} for user {AssignedToId} completed successfully", request.TaskId, request.ProjectId, request.AssignedToId);

            return task.Adapt<TaskDto>();
        }
    }
}
