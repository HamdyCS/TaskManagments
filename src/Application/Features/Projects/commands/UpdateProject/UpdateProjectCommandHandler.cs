using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateProjectCommandHandler> logger) : IRequestHandler<UpdateProjectCommand, ErrorOr<Success>>
    {
        public async Task<ErrorOr<Success>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting UpdateProject with id {ProjectId} in workSpace {WorkSpaceId}", request.ProjectId, request.WorkSpaceId);


            Project? project = await unitOfWork.ProjectRepository.GetByIdAndWorkSpaceIdAsync(request.ProjectId,request.WorkSpaceId);

            if (project is null)
            {
                logger.LogWarning("UpdateProject with id {ProjectId} not found in workSpace {WorkSpaceId}", request.ProjectId, request.WorkSpaceId);
                return ProjectErrors.ProjectNotFoundById(request.ProjectId);
            }

            // Update project fields
            project.Name = request.UpdateProjectDto.Name;
            project.Description = request.UpdateProjectDto.Description;

            if (request.UpdateProjectDto.Status.HasValue)
                project.Status = request.UpdateProjectDto.Status.Value;

            // FR-015: Track who updated and when
            project.LastUpdatedById = request.UserId;
            project.LastUpdatedAt = DateTime.UtcNow;

            unitOfWork.ProjectRepository.Update(project);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("UpdateProject with id {ProjectId} in workSpace {WorkSpaceId} by user {UserId} successfully ", request.ProjectId, request.WorkSpaceId, request.UserId);

            return Result.Success;
        }
    }
}
