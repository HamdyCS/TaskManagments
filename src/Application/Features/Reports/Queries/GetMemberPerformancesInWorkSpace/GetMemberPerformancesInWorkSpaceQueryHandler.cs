using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetMemberPerformancesInWorkSpace
{
    public class GetMemberPerformancesInWorkSpaceQueryHandler(
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ILogger<GetMemberPerformancesInWorkSpaceQueryHandler> logger) : IRequestHandler<GetMemberPerformancesInWorkSpaceQuery, ErrorOr<MemberPerformanceDto>>
    {
        public async Task<ErrorOr<MemberPerformanceDto>> Handle(GetMemberPerformancesInWorkSpaceQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Get member performances in workspace with workspaceId {WorkspaceId}", request.WorkspaceId);

            var workspace = await unitOfWork.WorkSpaceRepository.GetByIdAsync(request.WorkspaceId);
            if (workspace is null)
                return WorkSpaceErrors.WorkSpaceNotFoundById(request.WorkspaceId);

            var cacheKey = $"report:member-performances-ws:{request.WorkspaceId}";

            var cachedResult = await cacheService.GetAsync<MemberPerformanceDto>(cacheKey);
            if (cachedResult is not null)
            {
                logger.LogInformation("Got member performances for workspace {WorkspaceId} returned from cache", request.WorkspaceId);
                return cachedResult;
            }

            var report = await unitOfWork.ReportRepository.GetMemberPerformancesInWorkSpaceAsync(request.WorkspaceId);

            await cacheService.SetAsync(cacheKey, report, TimeSpan.FromMinutes(10));

            logger.LogInformation("Get member performances in workspace with workspaceId {WorkspaceId} successfully", request.WorkspaceId);

            return report;
        }
    }
}
