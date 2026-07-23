using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using ErrorOr;
using MediatR;

namespace Application.Features.Projects.Commands.UpdateProjectStatus
{
    public class UpdateProjectStatusCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateProjectStatusCommandHandler> logger) : IRequestHandler<UpdateProjectStatusCommand, ErrorOr<Success>>
    {
        public async Task<ErrorOr<Success>> Handle(UpdateProjectStatusCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting UpdateProjectStatus with id {ProjectId} in workSpace {WorkSpaceId} by user {UserId}", request.ProjectId, request.WorkSpaceId, request.UserId);

            var rowsAffected = await unitOfWork.ProjectRepository.UpdateStatusAsync(
                request.ProjectId,
                request.WorkSpaceId,
                request.UserId,
                request.UpdateProjectStatusDto.Status);

            if (rowsAffected == 0)
            {
                logger.LogWarning("UpdateProjectStatus with id {ProjectId} not found in workSpace {WorkSpaceId}", request.ProjectId, request.WorkSpaceId);
                return ProjectErrors.ProjectNotFoundById(request.ProjectId);
            }

            logger.LogInformation("UpdateProjectStatus with id {ProjectId} in workSpace {WorkSpaceId} by user {UserId} successfully", request.ProjectId, request.WorkSpaceId, request.UserId);

            return Result.Success;
        }
    }
}
