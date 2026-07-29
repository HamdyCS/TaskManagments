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
        ILogger<GetMemberPerformanceInWorkSpaceQueryHandler> logger) : IRequestHandler<GetMemberPerformanceInWorkSpaceQuery, ErrorOr<MemberPerformance>>
    {
        public async Task<ErrorOr<MemberPerformance>> Handle(GetMemberPerformanceInWorkSpaceQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetMemberPerformanceInWorkSpace with workspaceId {WorkspaceId} and memberId {MemberId}", request.WorkspaceId, request.MemberId);

            var workspace = await unitOfWork.WorkSpaceRepository.GetByIdAsync(request.WorkspaceId);
            if (workspace is null)
                return WorkSpaceErrors.WorkSpaceNotFoundById(request.WorkspaceId);

            var cacheKey = $"report:member-perf-ws:{request.WorkspaceId}:{request.MemberId}";

            var cachedResult = await cacheService.GetAsync<MemberPerformance>(cacheKey);
            if (cachedResult is not null)
            {
                logger.LogInformation("GetMemberPerformanceInWorkSpace with workspaceId {WorkspaceId} and memberId {MemberId} returned from cache", request.WorkspaceId, request.MemberId);
                return cachedResult;
            }

            var report = await unitOfWork.ReportRepository.GetMemberPerformanceInWorkSpaceAsync(request.WorkspaceId, request.MemberId);

            await cacheService.SetAsync(cacheKey, report, TimeSpan.FromMinutes(10));

            logger.LogInformation("GetMemberPerformanceInWorkSpace with workspaceId {WorkspaceId} and memberId {MemberId} successfully", request.WorkspaceId, request.MemberId);

            return report;
        }
    }
}
