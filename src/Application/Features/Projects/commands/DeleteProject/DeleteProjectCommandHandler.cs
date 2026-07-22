using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Features.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler(
        IUnitOfWork unitOfWork,
        IWorkSpaceService workSpaceService,
        ILogger<DeleteProjectCommandHandler> logger) : IRequestHandler<DeleteProjectCommand, ErrorOr<Success>>
    {
        public async Task<ErrorOr<Success>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting DeleteProject with id {ProjectId} in workSpace {WorkSpaceId}", request.ProjectId, request.WorkSpaceId);

         
            var project = await unitOfWork.ProjectRepository.GetByIdAndWorkSpaceIdAsync(request.ProjectId,request.WorkSpaceId);

            if (project is null)
            {
                logger.LogWarning("DeleteProject with id {ProjectId} not found in workSpace {WorkSpaceId}", request.ProjectId, request.WorkSpaceId);
                return ProjectErrors.ProjectNotFoundById(request.ProjectId);
            }

            // FR-014: Soft-delete via GenericRepository.Delete
            unitOfWork.ProjectRepository.Delete(project);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("DeleteProject with id {ProjectId} in workSpace {WorkSpaceId} successfully by user {UserId}", request.ProjectId, request.WorkSpaceId, request.UserId);

            return Result.Success;
        }
    }
}
