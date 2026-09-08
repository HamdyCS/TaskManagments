using Application.Common.Dtos;
using Application.Common.Dtos.AdminDashboard;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.AdminDashboard.Queries.GetRecentActivities
{
    public class GetRecentActivitiesQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetRecentActivitiesQueryHandler> logger) : IRequestHandler<GetRecentActivitiesQuery, ErrorOr<PaginationResultDto<RecentActivityDto>>>
    {
        public async Task<ErrorOr<PaginationResultDto<RecentActivityDto>>> Handle(GetRecentActivitiesQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PaginationRequestDto.PageNumber;
            var pageSize = request.PaginationRequestDto.PageSize;

            logger.LogInformation("Starting get recent activities with page number {PageNumber} and page size {PageSize}", pageNumber, pageSize);


            logger.LogInformation("Getting recent activities from database with page number {PageNumber} and page size {PageSize}", pageNumber, pageSize);
            var recentActivities = await unitOfWork.RecentActivityRepository.GetAllAsync(pageNumber, pageSize);

            logger.LogInformation("Get recent activities completed successfully with {TotalCount} total items", recentActivities.TotalCount);

            return recentActivities.Adapt<PaginationResultDto<RecentActivityDto>>();
        }
    }
}
