using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Features.Notifications.Command.CreateNotification;
using Domain.Common.Enums;
using ErrorOr;
using Mapster;
using MediatR;

namespace Application.Features.Tasks.Commands.ChangeTaskStatus
{
    public class ChangeTaskStatusCommandHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        ILogger<ChangeTaskStatusCommandHandler> logger) : IRequestHandler<ChangeTaskStatusCommand, ErrorOr<TaskDto>>
    {
        private static readonly Dictionary<ProjectTaskStatus, ProjectTaskStatus> ValidTransitions =
            new()
        {
            { ProjectTaskStatus.Backlog, ProjectTaskStatus.Todo },
            { ProjectTaskStatus.Todo, ProjectTaskStatus.InProgress },
            { ProjectTaskStatus.InProgress, ProjectTaskStatus.Review },
            { ProjectTaskStatus.Review, ProjectTaskStatus.Done },
            { ProjectTaskStatus.Done, ProjectTaskStatus.Backlog }
        };

        public async Task<ErrorOr<TaskDto>> Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting ChangeTaskStatus for task {TaskId} in project {ProjectId} by user with id {UserId}", request.TaskId, request.ProjectId, request.UserId);

            // Verify project belongs to workspace
            var project = await unitOfWork.ProjectRepository.GetByIdAndWorkSpaceIdAsync(request.ProjectId, request.WorkSpaceId);
            if (project is null)
                return TaskErrors.ProjectNotInWorkspace(request.ProjectId, request.WorkSpaceId);

            var task = await unitOfWork.TaskRepository.GetByIdAsync(request.TaskId);
            if (task is null || task.ProjectId != request.ProjectId)
                return TaskErrors.TaskNotFound(request.TaskId);

            // Validate status transition
            if (!ValidTransitions.TryGetValue(task.TaskStatus, out var validNextStatus) ||
                validNextStatus != request.ChangeTaskStatusDto.Status)
            {
                return TaskErrors.InvalidStatusTransition(task.TaskStatus, request.ChangeTaskStatusDto.Status);
            }

            task.TaskStatus = request.ChangeTaskStatusDto.Status;
            task.LastUpdatedAt = DateTime.UtcNow;
            task.LastUpdatedById = request.UserId;

            unitOfWork.TaskRepository.Update(task);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // notify active assignee
            var assignment = await unitOfWork.TaskAssignmentRepository.GetActiveAssignmentByTaskIdAsync(request.TaskId);
            if (assignment is not null)
            {
                await mediator.Send(new CreateNotificationCommand(new CreateNotificationDto(
                    assignment.AssignedToId,
                    request.TaskId,
                    null,
                    "Task Status Updated",
                    $"Task '{task.Name}' status changed to {request.ChangeTaskStatusDto.Status}",
                    NotificationType.TaskStatusUpdated)), cancellationToken);
            }

            logger.LogInformation("ChangeTaskStatus for task {TaskId} in project {ProjectId} by user with id {UserId} successfully", request.TaskId, request.ProjectId, request.UserId);

            return task.Adapt<TaskDto>();
        }
    }
}
