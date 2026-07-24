using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Features.Notifications.Command.CreateNotification;
using Domain.Common.Enums;
using Domain.Entities;
using ErrorOr;
using Mapster;
using MediatR;

namespace Application.Features.Tasks.Commands.AssignUsers
{
    public class AssignUserCommandHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        ILogger<AssignUserCommandHandler> logger) : IRequestHandler<AssignUserCommand, ErrorOr<TaskAssignmentDto>>
    {
        public async Task<ErrorOr<TaskAssignmentDto>> Handle(AssignUserCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting AssignUser to task {TaskId} in project {ProjectId} by user with id {UserId}", request.TaskId, request.ProjectId, request.UserId);

            // Verify project belongs to workspace
            var project = await unitOfWork.ProjectRepository.GetByIdAndWorkSpaceIdAsync(request.ProjectId, request.WorkSpaceId);
            if (project is null)
                return TaskErrors.ProjectNotInWorkspace(request.ProjectId, request.WorkSpaceId);

            var task = await unitOfWork.TaskRepository.GetByIdAndProjectIdAsync(request.TaskId, request.ProjectId);
            if (task is null || task.ProjectId != request.ProjectId)
                return TaskErrors.TaskNotFound(request.TaskId);

            //check is task has already been assigned
            var existingAssignment = await unitOfWork.TaskAssignmentRepository.GetActiveAssignmentByTaskIdAsync(request.TaskId);
            if (existingAssignment is not null)
                return TaskErrors.DuplicateAssignment(request.UserId);


            var newAssignment = new TaskAssignment
            {
                TaskId = request.TaskId,
                AssignedToId = request.AssignUserDto.UserId,
                AssignedById = request.UserId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            unitOfWork.TaskAssignmentRepository.Add(newAssignment);
            var isAdded = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!isAdded)
                return TaskErrors.TaskAssignmentFailed(request.AssignUserDto.UserId, request.TaskId);

            // Send notifications to newly assigned user        
            await mediator.Send(new CreateNotificationCommand(new CreateNotificationDto(
                request.AssignUserDto.UserId,
                request.TaskId,
                null,
                "Task Assigned",
                $"You have been assigned to task '{task.Name}'",
                NotificationType.TaskAssigned)), cancellationToken);


            logger.LogInformation("AssignUsers to task {TaskId} in project {ProjectId} by user with id {UserId} successfully", request.TaskId, request.ProjectId, request.UserId);

            return newAssignment.Adapt<TaskAssignmentDto>();
        }
    }
}
