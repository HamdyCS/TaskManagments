using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetWorkSpaceReport
{
    public class GetWorkSpaceReportQueryHandler(
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ILogger<GetWorkSpaceReportQueryHandler> logger) : IRequestHandler<GetWorkSpaceReportQuery, ErrorOr<WorkSpaceReportDto>>
    {
        public async Task<ErrorOr<WorkSpaceReportDto>> Handle(GetWorkSpaceReportQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetWorkSpaceReport with workspaceId {WorkspaceId}", request.WorkspaceId);

            var workspace = await unitOfWork.WorkSpaceRepository.GetByIdAsync(request.WorkspaceId);
            if (workspace is null)
                return WorkSpaceErrors.WorkSpaceNotFoundById(request.WorkspaceId);

            var cacheKey = $"report:workspace:{request.WorkspaceId}";

            var cachedResult = await cacheService.GetAsync<WorkSpaceReportDto>(cacheKey);
            if (cachedResult is not null)
            {
                logger.LogInformation("GetWorkSpaceReport with workspaceId {WorkspaceId} returned from cache", request.WorkspaceId);
                return cachedResult;
            }

            var report = await unitOfWork.ReportRepository.GetWorkSpaceReportAsync(request.WorkspaceId);

            await cacheService.SetAsync(cacheKey, report, TimeSpan.FromMinutes(10));

            logger.LogInformation("GetWorkSpaceReport with workspaceId {WorkspaceId} successfully", request.WorkspaceId);

            return report;
        }
    }
}
