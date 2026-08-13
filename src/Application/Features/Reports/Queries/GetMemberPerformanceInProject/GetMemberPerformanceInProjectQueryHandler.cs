using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetMemberPerformanceInProject
{
    public class GetMemberPerformanceInProjectQueryHandler(
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ILogger<GetMemberPerformanceInProjectQueryHandler> logger) : IRequestHandler<GetMemberPerformanceInProjectQuery, ErrorOr<MemberPerformanceDto>>
    {
        public async Task<ErrorOr<MemberPerformanceDto>> Handle(GetMemberPerformanceInProjectQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetMemberPerformanceInProject with projectId {ProjectId} and memberId {MemberId}", request.ProjectId, request.MemberId);

            var project = await unitOfWork.ProjectRepository.GetByIdAsync(request.ProjectId);
            if (project is null)
                return ProjectErrors.ProjectNotFoundById(request.ProjectId);

            var cacheKey = $"report:member-perf-proj:{request.ProjectId}:{request.MemberId}";

            var cachedResult = await cacheService.GetAsync<MemberPerformanceDto>(cacheKey);
            if (cachedResult is not null)
            {
                logger.LogInformation("GetMemberPerformanceInProject with projectId {ProjectId} and memberId {MemberId} returned from cache", request.ProjectId, request.MemberId);
                return cachedResult;
            }

            var report = await unitOfWork.ReportRepository.GetMemberPerformanceInProjectAsync(request.ProjectId, request.MemberId);

            await cacheService.SetAsync(cacheKey, report, TimeSpan.FromMinutes(10));

            logger.LogInformation("GetMemberPerformanceInProject with projectId {ProjectId} and memberId {MemberId} successfully", request.ProjectId, request.MemberId);

            return report;
        }
    }
}
