using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class WorkSpaceService(IUnitOfWork unitOfWork, ILogger<WorkSpaceService> logger) : IWorkSpaceService
    {
        public async Task<bool> IsWorkSpaceExistAsync(long workSpaceId)
        {
            logger.LogInformation("Starting check if workSpace with id {WorkSpaceId} exists", workSpaceId);

            var workSpace = await unitOfWork.WorkSpaceRepository.GetByIdAsync(workSpaceId);

            if (workSpace is null || workSpace.IsDeleted)
            {
                logger.LogWarning("WorkSpace with id {WorkSpaceId} not found or deleted", workSpaceId);
                return false;
            }

            logger.LogInformation("WorkSpace with id {WorkSpaceId} exists", workSpaceId);
            return true;
        }
    }
}
