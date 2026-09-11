using Application.Common.Dtos.WorkSpacesOverview;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetWorkSpacesOverviewReport
{
    public class GetWorkSpacesOverviewReportQueryHandler(
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ILogger<GetWorkSpacesOverviewReportQueryHandler> logger) : IRequestHandler<GetWorkSpacesOverviewReportQuery, ErrorOr<WorkSpacesOverviewReportDto>>
    {
        public async Task<ErrorOr<WorkSpacesOverviewReportDto>> Handle(GetWorkSpacesOverviewReportQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting get work spaces overview from {From} to {To}", request.QueryParameters.From, request.QueryParameters.To);

            var cacheKey = $"workspace:overview:{request.QueryParameters.From}:{request.QueryParameters.To}";

            var cachedResult = await cacheService.GetAsync<WorkSpacesOverviewReportDto>(cacheKey);
            if (cachedResult is not null)
            {
                logger.LogInformation("Got work spaces overview from {From} to {To} from cache", request.QueryParameters.From, request.QueryParameters.To);
                return cachedResult;
            }

            logger.LogInformation("Getting work spaces overview from {From} to {To}", request.QueryParameters.From, request.QueryParameters.To);
            var result = await unitOfWork.ReportRepository.GetWorkSpacesOverviewAsync(
                request.QueryParameters.From, request.QueryParameters.To);

            if (result is null)
                return WorkSpaceErrors.WorkSpaceNotFoundById(0);

            await cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));

            logger.LogInformation("Got work spaces overview from {From} to {To} successfully", request.QueryParameters.From, request.QueryParameters.To);

            return result;
        }
    }
}
