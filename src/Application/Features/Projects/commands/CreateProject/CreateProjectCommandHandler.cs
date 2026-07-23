using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Common.Enums;
using Domain.Entities;
using ErrorOr;
using Mapster;
using MediatR;

namespace Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler(
        IUnitOfWork unitOfWork,
        IWorkSpaceService workSpaceService,
        ILogger<CreateProjectCommandHandler> logger) : IRequestHandler<CreateProjectCommand, ErrorOr<ProjectDto>>
    {
        public async Task<ErrorOr<ProjectDto>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting CreateProject with name {Name} in workSpace {WorkSpaceId}", request.CreateProjectDto.Name, request.WorkSpaceId);

            // FR-011: Validate workspace exists
            if (!await workSpaceService.IsWorkSpaceExistAsync(request.WorkSpaceId))
                return ProjectErrors.WorkSpaceNotFound;

            // FR-010: Check name uniqueness (also validated in validator, but double-check for race conditions)
            if (!await unitOfWork.ProjectRepository.IsProjectNameUniqueInWorkspaceAsync(request.WorkSpaceId, request.CreateProjectDto.Name))
                return ProjectErrors.ProjectNameAlreadyExists(request.WorkSpaceId, request.CreateProjectDto.Name);

            // Create project entity
            var project = new Project
            {
                Name = request.CreateProjectDto.Name,
                Description = request.CreateProjectDto.Description,
                Status = ProjectStatus.Active,
                WorkSpaceId = request.WorkSpaceId,
                CreatedById = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            unitOfWork.ProjectRepository.Add(project);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("CreateProject with name {Name} in workSpace {WorkSpaceId} successfully by user {UserId}", request.CreateProjectDto.Name, request.WorkSpaceId, request.UserId);

            return project.Adapt<ProjectDto>();
        }
    }
}
