using Application.Common.Dtos;
using Application.Common.Dtos.WorkSpace;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Mapster;

namespace Application.Features.WorkSpaces.Queries.GetWorkSpaceOverviews
{
    public class GetAllWorkSpaceOverviewsQueryHandler(
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ILogger<GetAllWorkSpaceOverviewsQueryHandler> logger
    ) : IRequestHandler<GetAllWorkSpaceOverviewsQuery, ErrorOr<PaginationResultDto<WorkSpaceOverviewDto>>>
    {
        public async Task<ErrorOr<PaginationResultDto<WorkSpaceOverviewDto>>> Handle(
            GetAllWorkSpaceOverviewsQuery request,
            CancellationToken cancellationToken)
        {
            var pageNumber = request.PaginationRequestDto.PageNumber;
            var pageSize = request.PaginationRequestDto.PageSize;

            logger.LogInformation("Starting get workspace overviews");

            var cacheKey = $"workspace_overviews:{pageNumber}:{pageSize}:{request.OwnerNameQuery}:{request.WorkSpaceNameQuery}";

            logger.LogInformation("Checking cache for workspace overviews");
            var cachedWorkSpaceOverviews = await cacheService.GetAsync<PaginationResultDto<WorkSpaceOverviewDto>>(cacheKey);
            if (cachedWorkSpaceOverviews is not null)
            {
                logger.LogInformation("Returning cached workspace overviews");
                return cachedWorkSpaceOverviews;
            }

            logger.LogInformation("Getting workspace overviews");
            var result = await unitOfWork.WorkSpaceRepository.GetAllWorkSpaceOverviewsAsync(
               pageNumber, pageSize, request.OwnerNameQuery, request.WorkSpaceNameQuery);

            var resultDto = result.Adapt<PaginationResultDto<WorkSpaceOverviewDto>>();
 
            //cache the workspace overviews for 5 minutes
            if (result is not null) {
                await cacheService.SetAsync(cacheKey, resultDto, TimeSpan.FromMinutes(5));
            }

            logger.LogInformation("Got workspace overviews successfully");
            return resultDto;
        }
    }
}