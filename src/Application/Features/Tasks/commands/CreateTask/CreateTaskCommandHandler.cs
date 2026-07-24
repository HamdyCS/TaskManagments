using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Features.Notifications.Command.CreateNotification;
using Application.Features.Tasks.Commands.AssignUsers;
using Domain.Common.Enums;
using Domain.Entities;
using ErrorOr;
using Mapster;
using MediatR;

namespace Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        ILogger<CreateTaskCommandHandler> logger) : IRequestHandler<CreateTaskCommand, ErrorOr<TaskDto>>
    {
        public async Task<ErrorOr<TaskDto>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting CreateTask with name {Name} in project {ProjectId} by user with id {UserId}", request.CreateTaskDto.Name, request.ProjectId, request.UserId);

            // Verify project exists and belongs to workspace
            var project = await unitOfWork.ProjectRepository.GetByIdAndWorkSpaceIdAsync(request.ProjectId, request.WorkSpaceId);
            if (project is null)
                return TaskErrors.ProjectNotFound(request.ProjectId);

            // Check name uniqueness
            if (!await unitOfWork.TaskRepository.IsTaskNameUniqueInProjectAsync(request.ProjectId, request.CreateTaskDto.Name))
                return TaskErrors.TaskNameAlreadyExists(request.ProjectId, request.CreateTaskDto.Name);


            //begin transaction
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            // Create task entity
            var task = new ProjectTask
            {
                Name = request.CreateTaskDto.Name,
                Description = request.CreateTaskDto.Description,
                Deadline = request.CreateTaskDto.Deadline,
                TaskStatus = ProjectTaskStatus.Backlog,
                TaskPriority = request.CreateTaskDto.Priority,
                ProjectId = request.ProjectId,
                CreatedById = request.UserId,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                LastUpdatedById = request.UserId
            };

            unitOfWork.TaskRepository.Add(task);

            var isTaskAdded = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!isTaskAdded)
            {
                logger.LogWarning("Failed to create task with name {Name} in project {ProjectId} by user with id {UserId}", request.CreateTaskDto.Name, request.ProjectId, request.UserId);
                return TaskErrors.CreateTaskFailed(request.ProjectId, request.UserId);
            }

            // Create assignments if provided
            if (request.CreateTaskDto.AssignedUserId is not null)
            {
              var result =   await mediator.Send(new AssignUserCommand(new AssignUsersDto(request.CreateTaskDto.AssignedUserId), request.WorkSpaceId, request.ProjectId,
                    task.Id, request.UserId));

                if(result.IsError)
                {
                    return result.Errors;
                }
            }

            // Commit transaction
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            logger.LogInformation("CreateTask with name {Name} in project {ProjectId} by user with id {UserId} successfully", request.CreateTaskDto.Name, request.ProjectId, request.UserId);
            return task.Adapt<TaskDto>();
        }
    }
}
