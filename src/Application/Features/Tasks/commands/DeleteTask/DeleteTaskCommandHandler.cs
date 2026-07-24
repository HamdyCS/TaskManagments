using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Features.Notifications.Command.CreateNotification;
using Domain.Common.Enums;
using ErrorOr;
using MediatR;

namespace Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        ILogger<DeleteTaskCommandHandler> logger) : IRequestHandler<DeleteTaskCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting DeleteTask with id {TaskId} in project {ProjectId} by user with id {UserId}", request.TaskId, request.ProjectId, request.UserId);

            // Verify project belongs to workspace
            var project = await unitOfWork.ProjectRepository.GetByIdAndWorkSpaceIdAsync(request.ProjectId, request.WorkSpaceId);
            if (project is null)
                return TaskErrors.ProjectNotInWorkspace(request.ProjectId, request.WorkSpaceId);

            var task = await unitOfWork.TaskRepository.GetByIdAsync(request.TaskId);
            if (task is null || task.ProjectId != request.ProjectId)
                return TaskErrors.TaskNotFound(request.TaskId);

            //begin transaction
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            // Notify notify active assign before soft-deleting
            var assignment = await unitOfWork.TaskAssignmentRepository.GetActiveAssignmentByTaskIdAsync(request.TaskId);
            if (assignment is not null)
            {
                await mediator.Send(new CreateNotificationCommand(new CreateNotificationDto(
                    assignment.AssignedToId,
                    request.TaskId,
                    null,
                    "Task Deleted",
                    $"Task '{task.Name}' has been deleted",
                    NotificationType.TaskDeleted)), cancellationToken);

                // Deactivate assignment
                assignment.IsActive = false;
                unitOfWork.TaskAssignmentRepository.Update(assignment);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

          
            // Soft-delete the task
            unitOfWork.TaskRepository.Delete(task);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Commit transaction
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            logger.LogInformation("DeleteTask with id {TaskId} in project {ProjectId} by user with id {UserId} successfully", request.TaskId, request.ProjectId, request.UserId);

            return true;
        }
    }
}
