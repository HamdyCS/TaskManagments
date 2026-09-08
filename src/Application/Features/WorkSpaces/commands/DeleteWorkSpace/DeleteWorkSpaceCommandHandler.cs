using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Common.Enums;
using Microsoft.Extensions.Logging;

namespace Application.Features.WorkSpaces.commands.DeleteWorkSpace
{
    public class DeleteWorkSpaceCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteWorkSpaceCommandHandler> logger,
        IRecentActivityService recentActivityService
        ) : IRequestHandler<DeleteWorkSpaceCommand, ErrorOr<bool>>

    {
        public async Task<ErrorOr<bool>> Handle(DeleteWorkSpaceCommand request, CancellationToken cancellationToken)
        {
            var deleteBy = request.DeleteBy;
            var workSpaceId = request.WorkSpaceId;
            logger.LogInformation("Starting Delete workspace with id {WorkSpaceId} by user with id {UserId}", workSpaceId, deleteBy);

            var workSpace = await unitOfWork.WorkSpaceRepository.GetByIdAsync(workSpaceId);
            if (workSpace is null)
            {
                logger.LogWarning("Workspace with id {WorkSpaceId} not found", workSpaceId);
                return WorkSpaceErrors.WorkSpaceNotFoundById(workSpaceId);
            }

            //delete workspace
            logger.LogInformation("Deleting workspace with id {WorkSpaceId} by user with id {UserId}", workSpaceId, deleteBy);


            unitOfWork.WorkSpaceRepository.Delete(workSpace);

            var isDeletedWorkSpace = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!isDeletedWorkSpace)
            {
                logger.LogWarning("Failed to delete workspace with id {WorkSpaceId} by user with id {UserId}", workSpaceId, deleteBy);
                return WorkSpaceErrors.DeleteWorkSpaceFailed(workSpaceId, deleteBy);
            }

            //add recent activity
            var fullName = await unitOfWork.UserRepository.GetUserFullNameAsync(deleteBy, cancellationToken);
            var recentActivity = new RecentActivity
            {
                Text = $"{fullName} deleted workspace {workSpace.Name}",
                ActivityType = RecentActivityType.WorkSpaceDeleted,
                CreatedAt = DateTime.UtcNow
            };


            logger.LogInformation("Deleted workspace with id {WorkSpaceId} by user with id {UserId} ", workSpaceId, deleteBy);
            return true;

        }
    }
}
