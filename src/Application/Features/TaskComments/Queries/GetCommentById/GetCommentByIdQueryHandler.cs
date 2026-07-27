using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using Mapster;

namespace Application.Features.TaskComments.Queries.GetCommentById
{
    public class GetCommentByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ILogger<GetCommentByIdQueryHandler> logger) : IRequestHandler<GetCommentByIdQuery, ErrorOr<TaskCommentDto>>
    {
        public async Task<ErrorOr<TaskCommentDto>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetCommentById with id {CommentId} for task {TaskId}", request.CommentId, request.TaskId);

            var task = await unitOfWork.TaskRepository.GetByIdAndWorkSpaceIdAndProjectIdAsync(request.TaskId, request.WorkSpaceId, request.ProjectId);
            if (task is null)
                return TaskErrors.TaskNotFound(request.TaskId);

            // Try cache first
            var cacheKey = $"TaskComments_{request.TaskId}";

            var cachedResult = await cacheService.GetAsync<IEnumerable<TaskCommentDto>>(cacheKey);
            var cachedComment = cachedResult?.FirstOrDefault(c => c.Id == request.CommentId);
            if (cachedComment is not null)
            {
                logger.LogInformation("GetCommentById with id {CommentId} returned from cache", request.CommentId);
                return cachedComment;
            }


            var comment = await unitOfWork.TaskCommentRepository.GetByTaskIdAndIdAsync(request.TaskId,
                request.CommentId);
            if (comment is null)
                return TaskCommentErrors.NotFound(request.CommentId);

            //update Cache 
            var taskComments = await unitOfWork.TaskCommentRepository.GetAllWithoutPaginationByTaskIdAsync(request.TaskId);
            if (taskComments.Any())
            {
                var taskCommentsDtoList = taskComments.Adapt<List<TaskCommentDto>>();
                await cacheService.SetAsync(cacheKey, taskCommentsDtoList, TimeSpan.FromHours(24));
            }

            logger.LogInformation("GetCommentById with id {CommentId} for task {TaskId} successfully", request.CommentId, request.TaskId);
            return comment.Adapt<TaskCommentDto>();
        }
    }
}
