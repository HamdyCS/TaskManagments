using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetMemberPerformanceInWorkSpace
{
    public class GetMemberPerformanceInWorkSpaceQueryHandler(
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ILogger<GetMemberPerformanceInWorkSpaceQueryHandler> logger) : IRequestHandler<GetMemberPerformanceInWorkSpaceQuery, ErrorOr<MemberPerformanceDto>>
    {
        public async Task<ErrorOr<MemberPerformanceDto>> Handle(GetMemberPerformanceInWorkSpaceQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetMemberPerformanceInWorkSpace with workspaceId {WorkspaceId}", request.WorkspaceId);

            var workspace = await unitOfWork.WorkSpaceRepository.GetByIdAsync(request.WorkspaceId);
            if (workspace is null)
                return WorkSpaceErrors.WorkSpaceNotFoundById(request.WorkspaceId);

            var cacheKey = $"report:member-perf-ws:{request.WorkspaceId}";

            var cachedResult = await cacheService.GetAsync<MemberPerformanceDto>(cacheKey);
            if (cachedResult is not null)
            {
                logger.LogInformation("GetMemberPerformanceInWorkSpace with workspaceId {WorkspaceId} returned from cache", request.WorkspaceId);
                return cachedResult;
            }

            var report = await unitOfWork.ReportRepository.GetMemberPerformanceInWorkSpaceAsync(request.WorkspaceId);

            await cacheService.SetAsync(cacheKey, report, TimeSpan.FromMinutes(10));

            logger.LogInformation("GetMemberPerformanceInWorkSpace with workspaceId {WorkspaceId} successfully", request.WorkspaceId);

            return report;
        }
    }
}
