using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using Mapster;
using MediatR;

namespace Application.Features.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsQueryHandler(
        IUnitOfWork unitOfWork,
        IWorkSpaceService workSpaceService,
        ILogger<GetAllProjectsQueryHandler> logger) : IRequestHandler<GetAllProjectsQuery, ErrorOr<PaginationResultDto<ProjectDto>>>
    {
        public async Task<ErrorOr<PaginationResultDto<ProjectDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetAllProjects in workSpace {WorkSpaceId}", request.WorkSpaceId);

            // FR-011: Validate workspace exists
            if (!await workSpaceService.IsWorkSpaceExistAsync(request.WorkSpaceId))
                return ProjectErrors.WorkSpaceNotFound;

            var paginationResult = await unitOfWork.ProjectRepository.GetAllByWorkSpaceIdAsync(
                request.WorkSpaceId,
                request.PaginationRequest.PageNumber,
                request.PaginationRequest.PageSize);

            logger.LogInformation("GetAllProjects in workSpace {WorkSpaceId} returned {Count} projects", request.WorkSpaceId, paginationResult.Data.Count());

            return paginationResult.Adapt<PaginationResultDto<ProjectDto>>();
        }
    }
}
