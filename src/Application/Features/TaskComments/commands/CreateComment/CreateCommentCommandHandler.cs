using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Features.Notifications.Command.CreateNotification;
using Domain.Common.Enums;
using Domain.Entities;
using ErrorOr;
using Mapster;

namespace Application.Features.TaskComments.Commands.CreateComment
{
    public class CreateCommentCommandHandler(
        IMediator mediator,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ILogger<CreateCommentCommandHandler> logger) : IRequestHandler<CreateCommentCommand, ErrorOr<TaskCommentDto>>
    {
        public async Task<ErrorOr<TaskCommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting CreateComment for task {TaskId} by user with id {UserId}", request.TaskId, request.UserId);

            //get task
            var task = await unitOfWork.TaskRepository.GetByIdAndWorkSpaceIdAndProjectIdAsync(request.TaskId, request.WorkSpaceId, request.ProjectId);
            if (task is null)
                return TaskErrors.TaskNotFound(request.TaskId);

            //get user full name
            string? userFullName = await unitOfWork.UserRepository.GetUserFullNameAsync(request.UserId, cancellationToken);
            var comment = new TaskComment
            {
                Comment = request.CreateCommentDto.Comment.Trim(),
                TaskId = request.TaskId,
                CommentById = request.UserId,
                CommentByName = userFullName ?? "Unknown User",
                CreatedAt = DateTime.UtcNow
            };

            unitOfWork.TaskCommentRepository.Add(comment);

            var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;
            if (!isSaved)
            {
                logger.LogWarning("Failed to save comment for task {TaskId} by user with id {UserId}", request.TaskId, request.UserId);
                return TaskCommentErrors.CreateFailed(comment.TaskId, request.UserId);
            }

            //update Cache 
            try
            {
                var cacheKey = $"TaskComments_{request.TaskId}";
                var cachedComments = await unitOfWork.TaskCommentRepository.GetAllWithoutPaginationByTaskIdAsync(request.TaskId);

                if (cachedComments.Any())
                {
                    var cachedCommentDtoList = cachedComments.Adapt<List<TaskCommentDto>>();
                    await cacheService.SetAsync(cacheKey, cachedCommentDtoList, TimeSpan.FromMinutes(5));
                }
               
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to update task comments cache for task {TaskId}", request.TaskId);
            }

            //notifiy active assigned user
            var activeAssignment = await unitOfWork.TaskAssignmentRepository.GetActiveAssignmentByTaskIdAsync(request.TaskId);

            if (activeAssignment is not null)
            {
                await mediator.Send(new CreateNotificationCommand(new CreateNotificationDto(
                    activeAssignment.AssignedToId, request.TaskId, null, $"" +
                    $"A new comment has been added to task {request.TaskId}.",
                    $"New comment has been added to task {request.TaskId}", NotificationType.CommentAdded)));
            }

            logger.LogInformation("CreateComment for task {TaskId} by user with id {UserId} successfully", request.TaskId, request.UserId);
            return comment.Adapt<TaskCommentDto>();
        }
    }
}
