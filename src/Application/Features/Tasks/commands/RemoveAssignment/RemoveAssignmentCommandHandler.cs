using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Features.Notifications.Command.CreateNotification;
using Domain.Common.Enums;
using ErrorOr;
using MediatR;

namespace Application.Features.Tasks.Commands.RemoveAssignment
{
    public class RemoveAssignmentCommandHandler(IMediator mediator,
        IUnitOfWork unitOfWork,
        ILogger<RemoveAssignmentCommandHandler> logger) : IRequestHandler<RemoveAssignmentCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(RemoveAssignmentCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting RemoveAssignment for user {AssignedUserId} from task {TaskId} in project {ProjectId} by user with id {UserId}", request.AssignedUserId, request.TaskId, request.ProjectId, request.UserId);

            // Verify project belongs to workspace
            var project = await unitOfWork.ProjectRepository.GetByIdAndWorkSpaceIdAsync(request.ProjectId, request.WorkSpaceId);
            if (project is null)
                return TaskErrors.ProjectNotInWorkspace(request.ProjectId, request.WorkSpaceId);

            var task = await unitOfWork.TaskRepository.GetByIdAndProjectIdAsync(request.TaskId, request.ProjectId);
            if (task is null || task.ProjectId != request.ProjectId)
                return TaskErrors.TaskNotFound(request.TaskId);

            var assignment = await unitOfWork.TaskAssignmentRepository.GetByTaskIdAndAssignedToIdAsync(request.TaskId, request.AssignedUserId);
            if (assignment is null)
                return TaskErrors.AssignmentNotFound();

            if (assignment.TaskId != request.TaskId || assignment.AssignedToId != request.AssignedUserId)
                return TaskErrors.AssignmentNotFound();


            //DesActivate assignment
            assignment.IsActive = false;
            assignment.UnassignedAt = DateTime.UtcNow;
            unitOfWork.TaskAssignmentRepository.Update(assignment);

            var isUpdated = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!isUpdated)
                return TaskErrors.RemoveAssignmentFailed(task.Id, request.AssignedUserId);



            //notify user that assignment is removed
            await mediator.Send(new
                CreateNotificationCommand(new CreateNotificationDto
               (

                   request.AssignedUserId,
                   request.TaskId,
                   null,
                   "Assignment removed",
                   $"You have been removed from task with name {task}",
                   NotificationType.TaskUnassigned
                )), cancellationToken);

            logger.LogInformation("RemoveAssignment for user {AssignedUserId} from task {TaskId} in project {ProjectId} by user with id {UserId} successfully", request.AssignedUserId, request.TaskId, request.ProjectId, request.UserId);

            return true;
        }
    }
}
