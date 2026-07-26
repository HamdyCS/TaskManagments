using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Common.Enums;
using Domain.Common.Pagination;
using Domain.Entities;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Tasks.Queries.GetAllProjectTasks
{
    public class GetAllProjectTasksQueryHandler(
        IFileUrlService fileUrlService,
        IUnitOfWork unitOfWork,
        ILogger<GetAllProjectTasksQueryHandler> logger) : IRequestHandler<GetAllProjectTasksQuery, ErrorOr<PaginationResultDto<TaskDto>>>
    {
        public async Task<ErrorOr<PaginationResultDto<TaskDto>>> Handle(GetAllProjectTasksQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetAllProjectTasks for project {ProjectId}", request.ProjectId);

            var project = await unitOfWork.ProjectRepository.GetByIdAndWorkSpaceIdAsync(request.ProjectId, request.WorkSpaceId);
            if (project is null)
                return TaskErrors.ProjectNotInWorkspace(request.ProjectId, request.WorkSpaceId);

            var filter = request.FilterParams;
            PaginationResult<ProjectTask> result;

            if (filter is not null && (filter.Status.HasValue || filter.Priority.HasValue || !string.IsNullOrWhiteSpace(filter.SearchTerm) || !string.IsNullOrWhiteSpace(filter.SortBy)))
            {
                result = await unitOfWork.TaskRepository.GetAllFilteredAsync(
                    request.ProjectId,
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
                result = await unitOfWork.TaskRepository.GetAllByProjectIdAsync(
                    request.ProjectId,
                    request.PaginationRequestDto.PageNumber,
                    request.PaginationRequestDto.PageSize);
            }

            var dtoResult = result.Adapt<PaginationResultDto<TaskDto>>();
            foreach (var item in dtoResult.Data)
            {

               item.Attachments.ForEach(attachment =>
               {
                   //attachment.Url now = the Path of the attachment, we need to convert it to a full URL
                   attachment.Url = fileUrlService.GetUrl(attachment.Url);
               });
            }

            logger.LogInformation("GetAllProjectTasks for project {ProjectId} completed successfully", request.ProjectId);

            return dtoResult;
        }
    }
}
