using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using Mapster;

namespace Application.Features.TaskComments.Commands.DeleteCommentByCommentedById
{
    public class DeleteCommentByCommentedByIdCommandHandler(
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ILogger<DeleteCommentByCommentedByIdCommandHandler> logger) : IRequestHandler<DeleteCommentByCommentedByIdCommand, ErrorOr<Deleted>>
    {
        public async Task<ErrorOr<Deleted>> Handle(DeleteCommentByCommentedByIdCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting DeleteComment with id {CommentId} for task {TaskId} by user with id {UserId}", request.CommentId, request.TaskId, request.CommentedById);

            var task = await unitOfWork.TaskRepository.GetByIdAndWorkSpaceIdAndProjectIdAsync(request.TaskId, request.WorkSpaceId, request.ProjectId);
            if (task is null)
                return TaskErrors.TaskNotFound(request.TaskId);

            var comment = await unitOfWork.TaskCommentRepository.GetByTaskIdAndCommentedByIdAndIdAsync(request.TaskId, request.CommentedById, request.CommentId);
            if (comment is null)
                return TaskCommentErrors.NotFound(request.CommentId);

            unitOfWork.TaskCommentRepository.Delete(comment);

            var isSaved = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;
            if (!isSaved)
            {
                logger.LogWarning("Failed to delete comment {CommentId} for task {TaskId}", request.CommentId, request.TaskId);
                return TaskCommentErrors.DeleteFailed(request.CommentId, request.TaskId, request.CommentedById);
            }

            //update Cache 
            try
            {
                var cacheKey = $"TaskComments_{request.TaskId}";
                var cachedComments = await unitOfWork.TaskCommentRepository.GetAllWithoutPaginationByTaskIdAsync(request.TaskId);

                if (cachedComments.Any())
                {
                    var cachedCommentDtoList = cachedComments.Adapt<List<TaskCommentDto>>();
                    await cacheService.SetAsync(cacheKey, cachedCommentDtoList, TimeSpan.FromMinutes(10));
                }

            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to update task comments cache for task {TaskId}", request.TaskId);
            }

            logger.LogInformation("DeleteComment with id {CommentId} for task {TaskId} by user with id {UserId} successfully", request.CommentId, request.TaskId, request.CommentedById);
            return Result.Deleted;
        }
    }
}
