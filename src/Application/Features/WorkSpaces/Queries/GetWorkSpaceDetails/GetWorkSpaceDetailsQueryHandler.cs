using Application.Common.Dtos.WorkSpace;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Mapster;

namespace Application.Features.WorkSpaces.Queries.GetWorkSpaceDetails
{
    public class GetWorkSpaceDetailsQueryHandler(
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ILogger<GetWorkSpaceDetailsQueryHandler> logger
    ) : IRequestHandler<GetWorkSpaceDetailsQuery, ErrorOr<WorkSpaceDetailsDto>>
    {
        public async Task<ErrorOr<WorkSpaceDetailsDto>> Handle(
            GetWorkSpaceDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var workSpaceId = request.WorkSpaceId;
            logger.LogInformation("Starting get workspace details for workspace {WorkSpaceId}", workSpaceId);

            //try to get workspace details from cache first
            var cacheKey = $"workspace_details:{workSpaceId}";


            logger.LogInformation("Checking cache for workspace details for workspace {WorkSpaceId}", workSpaceId);
            var cachedWorkSpace = await cacheService.GetAsync<WorkSpaceDetailsDto>(cacheKey);
            if (cachedWorkSpace is not null)
            {
                logger.LogInformation("Returning cached workspace details for workspace {WorkSpaceId}", workSpaceId);
                return cachedWorkSpace;
            }

            logger.LogInformation("Getting workspace details for workspace {WorkSpaceId}", workSpaceId);
            var workSpaceDetailsDto = await unitOfWork.WorkSpaceRepository.GetWorkSpaceDetailsAsync(workSpaceId);

            if (workSpaceDetailsDto is null)
            {
                logger.LogWarning("Workspace {WorkSpaceId} not found", workSpaceId);
                return WorkSpaceErrors.WorkSpaceNotFoundById(workSpaceId);
            }

            //cache the workspace details for 5 minutes
            await cacheService.SetAsync(cacheKey, workSpaceDetailsDto, TimeSpan.FromMinutes(5));

            logger.LogInformation("Getting workspace details for workspace {WorkSpaceId} successfully", workSpaceId);
            return workSpaceDetailsDto;
        }
    }
}