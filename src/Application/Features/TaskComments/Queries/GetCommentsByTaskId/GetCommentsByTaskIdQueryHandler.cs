using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Extensions;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using Mapster;

namespace Application.Features.TaskComments.Queries.GetCommentsByTaskId
{
    public class GetCommentsByTaskIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ILogger<GetCommentsByTaskIdQueryHandler> logger) : IRequestHandler<GetCommentsByTaskIdQuery, ErrorOr<PaginationResultDto<TaskCommentDto>>>
    {
        public async Task<ErrorOr<PaginationResultDto<TaskCommentDto>>> Handle(GetCommentsByTaskIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetCommentsByTaskId for task {TaskId}", request.TaskId);

            var task = await unitOfWork.TaskRepository.GetByIdAndWorkSpaceIdAndProjectIdAsync(request.TaskId, request.WorkSpaceId, request.ProjectId);
            if (task is null)
                return TaskErrors.TaskNotFound(request.TaskId);

            // Try cache first (single key per task, no pagination in cache key)
            var cacheKey = $"TaskComments_{request.TaskId}";
            try
            {
                var cachedComments = await cacheService.GetAsync<List<TaskCommentDto>>(cacheKey);
                if (cachedComments is not null)
                {
                    logger.LogInformation("GetCommentsByTaskId for task {TaskId} returned from cache", request.TaskId);
                    return cachedComments.ToPaginationResultDto(request.PageNumber, request.PageSize);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to get cache for task {TaskId}", request.TaskId);
            }

            var comments = await unitOfWork.TaskCommentRepository.GetAllByTaskIdAsync(request.TaskId, request.PageNumber, request.PageSize);

            var commentsDtos = comments.Adapt<PaginationResultDto<TaskCommentDto>>();

            //store in cache for future requests
            try
            {
                await cacheService.SetAsync(cacheKey, commentsDtos.Data, TimeSpan.FromMinutes(10));
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to set cache for task {TaskId}", request.TaskId);
            }

            logger.LogInformation("GetCommentsByTaskId for task {TaskId} successfully", request.TaskId);
            return commentsDtos;
        }

        
    }
}

