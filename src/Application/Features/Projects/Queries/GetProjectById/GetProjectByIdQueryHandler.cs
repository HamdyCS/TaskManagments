using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using Mapster;
using MediatR;

namespace Application.Features.Projects.Queries.GetProjectById
{
    public class GetProjectByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IWorkSpaceService workSpaceService,
        ILogger<GetProjectByIdQueryHandler> logger) : IRequestHandler<GetProjectByIdQuery, ErrorOr<ProjectDto>>
    {
        public async Task<ErrorOr<ProjectDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetProjectById with id {ProjectId} in workSpace {WorkSpaceId}", request.ProjectId, request.WorkSpaceId);


            var project = await unitOfWork.ProjectRepository.GetByIdAndWorkSpaceIdAsync(request.ProjectId,request.WorkSpaceId);

            if (project is null)
            {
                logger.LogWarning("GetProjectById with id {ProjectId} not found in workSpace {WorkSpaceId}", request.ProjectId, request.WorkSpaceId);
                return ProjectErrors.ProjectNotFoundById(request.ProjectId);
            }

            logger.LogInformation("GetProjectById with id {ProjectId} in workSpace {WorkSpaceId} successfully", request.ProjectId, request.WorkSpaceId);

            return project.Adapt<ProjectDto>();
        }
    }
}
