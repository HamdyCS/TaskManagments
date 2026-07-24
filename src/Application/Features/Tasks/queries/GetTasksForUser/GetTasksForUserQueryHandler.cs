using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.Common.Pagination;
using Domain.Entities;
using ErrorOr;
using Mapster;
using MediatR;

namespace Application.Features.Tasks.Queries.GetTasksForUser
{
    public class GetTasksForUserQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetTasksForUserQueryHandler> logger) : IRequestHandler<GetTasksForUserQuery, ErrorOr<PaginationResultDto<TaskDto>>>
    {
        public async Task<ErrorOr<PaginationResultDto<TaskDto>>> Handle(GetTasksForUserQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetTasksForUser for user {UserId} in project {ProjectId}", request.UserId, request.ProjectId);

            var project = await unitOfWork.ProjectRepository.GetByIdAndWorkSpaceIdAsync(request.ProjectId, request.WorkSpaceId);
            if (project is null)
                return TaskErrors.ProjectNotInWorkspace(request.ProjectId, request.WorkSpaceId);

            var filter = request.FilterParams;
            PaginationResult<ProjectTask> result;

            if (filter is not null && (filter.Status.HasValue || filter.Priority.HasValue || !string.IsNullOrWhiteSpace(filter.SearchTerm) || !string.IsNullOrWhiteSpace(filter.SortBy)))
            {
                result = await unitOfWork.TaskRepository.GetByProjectIdAndUserIdFilteredAsync(
                    request.ProjectId,
                    request.UserId,
                    request.PaginationRequestDto.PageNumber,
                    request.PaginationRequestDto.PageSize,
                    filter.Status,
                    filter.Priority,
                    filter.SearchTerm,
                    filter.SortBy,
                    filter.SortOrder);
            }
            else
            {
                result = await unitOfWork.TaskRepository.GetByProjectIdAndUserIdAsync(
                    request.ProjectId,
                    request.UserId,
                    request.PaginationRequestDto.PageNumber,
                    request.PaginationRequestDto.PageSize);
            }

            var dtoResult = result.Adapt<PaginationResultDto<TaskDto>>();

            logger.LogInformation("GetTasksForUser for user {UserId} in project {ProjectId} completed successfully", request.UserId, request.ProjectId);

            return dtoResult;
        }
    }
}
