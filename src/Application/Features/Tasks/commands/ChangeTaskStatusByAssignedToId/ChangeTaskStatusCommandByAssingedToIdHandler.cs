using Application.Common.Errors;
using Application.Common.Extensions;
using Application.Common.Interfaces.Repositories;
using Application.Features.Notifications.Command.CreateNotification;
using Domain.Common.Enums;
using ErrorOr;
using Mapster;
using MediatR;

namespace Application.Features.Tasks.Commands.ChangeTaskStatusByAssignedToId
{
    public class ChangeTaskStatusCommandByAssingedToIdHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        ILogger<ChangeTaskStatusCommandByAssingedToIdHandler> logger) : IRequestHandler<ChangeTaskStatusCommandByAssignedToId, ErrorOr<TaskDto>>
    {
      

        public async Task<ErrorOr<TaskDto>> Handle(ChangeTaskStatusCommandByAssignedToId request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting ChangeTaskStatus for task {TaskId} in project {ProjectId} by user with id {UserId}", request.TaskId, request.ProjectId, request.AssignedToId);

            // Verify project belongs to workspace
            var project = await unitOfWork.ProjectRepository.GetByIdAndWorkSpaceIdAsync(request.ProjectId, request.WorkSpaceId);
            if (project is null)
                return TaskErrors.ProjectNotInWorkspace(request.ProjectId, request.WorkSpaceId);

            //get task
            var task = await unitOfWork.TaskRepository
                .GetByIdAndWorkSpaceIdAndProjectIdAndAssignedToIdAsync(request.TaskId, request.WorkSpaceId, request.ProjectId, request.AssignedToId);

            //check if task exists
            if (task is null)
                return TaskErrors.TaskNotFound(request.TaskId);

            // Validate status transition
            if (!task.TaskStatus.IsValidTransition(request.ChangeTaskStatusDto.Status))
            {
                return TaskErrors.InvalidStatusTransition(task.TaskStatus, request.ChangeTaskStatusDto.Status);
            }

            // Update task status
            task.TaskStatus = request.ChangeTaskStatusDto.Status;
            task.LastUpdatedAt = DateTime.UtcNow;
            task.LastUpdatedById = request.AssignedToId;

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

            logger.LogInformation("ChangeTaskStatus for task {TaskId} in project {ProjectId} by user with id {UserId} successfully", request.TaskId, request.ProjectId, request.AssignedToId);

            return task.Adapt<TaskDto>();
        }
    }
}
