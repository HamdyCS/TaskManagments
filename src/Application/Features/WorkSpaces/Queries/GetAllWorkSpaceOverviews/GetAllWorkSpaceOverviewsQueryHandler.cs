using Application.Common.Dtos;
using Application.Common.Dtos.WorkSpace;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Mapster;

namespace Application.Features.WorkSpaces.Queries.GetWorkSpaceOverviews
{
    public class GetAllWorkSpaceOverviewsQueryHandler(
        IUnitOfWork unitOfWork,
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

            logger.LogInformation("Getting workspace overviews");
            var result = await unitOfWork.WorkSpaceRepository.GetAllWorkSpaceOverviewsAsync(
               pageNumber, pageSize, request.OwnerNameQuery, request.WorkSpaceNameQuery);

            logger.LogInformation("Got workspace overviews successfully");
            return result.Adapt<PaginationResultDto<WorkSpaceOverviewDto>>();
        }
    }
}