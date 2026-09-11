using Application.Common.Dtos;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Common.Pagination;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetAllMemberPerformances
{
    public class GetAllMemberPerformancesQueryHandler(
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ILogger<GetAllMemberPerformancesQueryHandler> logger) : IRequestHandler<GetAllMemberPerformancesQuery, ErrorOr<PaginationResult<MemberPerformanceDto>>>
    {
        public async Task<ErrorOr<PaginationResult<MemberPerformanceDto>>> Handle(GetAllMemberPerformancesQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PaginationRequestDto.PageNumber;
            var pageSize = request.PaginationRequestDto.PageSize;

            logger.LogInformation("Starting get all member performances with pageNumber {PageNumber} and pageSize {PageSize}", pageNumber, pageSize);

            var cacheKey = $"report:all-member-performances:{pageNumber}:{pageSize}:{request.MemberNameQuery}";

            var cachedResult = await cacheService.GetAsync<PaginationResult<MemberPerformanceDto>>(cacheKey);
            if (cachedResult is not null)
            {
                logger.LogInformation("Got all member performances with pageNumber {PageNumber} and pageSize {PageSize} from cache", pageNumber, pageSize);
                return cachedResult;
            }

            var result = await unitOfWork.ReportRepository.GetAllMemberPerformancesAsync(
                pageNumber, pageSize, request.MemberNameQuery);

            await cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

            logger.LogInformation("Got all member performances with pageNumber {PageNumber} and pageSize {PageSize} successfully", pageNumber, pageSize);

            return result;
        }
    }
}
