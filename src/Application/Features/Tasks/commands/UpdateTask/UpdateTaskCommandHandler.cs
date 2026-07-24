using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Features.Notifications.Command.CreateNotification;
using Domain.Common.Enums;
using ErrorOr;
using Mapster;
using MediatR;

namespace Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        ILogger<UpdateTaskCommandHandler> logger) : IRequestHandler<UpdateTaskCommand, ErrorOr<TaskDto>>
    {
        public async Task<ErrorOr<TaskDto>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting UpdateTask with id {TaskId} in project {ProjectId} by user with id {UserId}", request.TaskId, request.ProjectId, request.UserId);

            // Verify project belongs to workspace
            var project = await unitOfWork.ProjectRepository.GetByIdAndWorkSpaceIdAsync(request.ProjectId, request.WorkSpaceId);
            if (project is null)
                return TaskErrors.ProjectNotInWorkspace(request.ProjectId, request.WorkSpaceId);

            var task = await unitOfWork.TaskRepository.GetByIdAsync(request.TaskId);
            if (task is null || task.ProjectId != request.ProjectId)
                return TaskErrors.TaskNotFound(request.TaskId);

            // Update fields if provided
            if (request.UpdateTaskDto.Name is not null)
            {
                if (!await unitOfWork.TaskRepository.IsTaskNameUniqueInProjectAsync(request.ProjectId, request.UpdateTaskDto.Name, request.TaskId))
                    return TaskErrors.TaskNameAlreadyExists(request.ProjectId, request.UpdateTaskDto.Name);

                task.Name = request.UpdateTaskDto.Name;
            }

            if (request.UpdateTaskDto.Description is not null)
                task.Description = request.UpdateTaskDto.Description;

            if (request.UpdateTaskDto.Deadline.HasValue)
                task.Deadline = request.UpdateTaskDto.Deadline;

            if (request.UpdateTaskDto.Priority.HasValue)
                task.TaskPriority = request.UpdateTaskDto.Priority.Value;

            task.LastUpdatedAt = DateTime.UtcNow;
            task.LastUpdatedById = request.UserId;

            unitOfWork.TaskRepository.Update(task);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Notify all assigned users of task update
            var assignment = await unitOfWork.TaskAssignmentRepository.GetActiveAssignmentByTaskIdAsync(request.TaskId);
            if (assignment is not null)
            {
                await mediator.Send(new CreateNotificationCommand(new CreateNotificationDto(
                    assignment.AssignedToId,
                    request.TaskId,
                    null,
                    "Task Updated",
                    $"Task '{task.Name}' has been updated",
                    NotificationType.TaskUpdated)), cancellationToken);
            }

    
            logger.LogInformation("UpdateTask with id {TaskId} in project {ProjectId} by user with id {UserId} successfully", request.TaskId, request.ProjectId, request.UserId);

            return task.Adapt<TaskDto>();
        }
    }
}
