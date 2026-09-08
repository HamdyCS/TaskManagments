using Application.Common.Dtos.AdminDashboard;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.AdminDashboard.Queries.GetAdminDashboard
{
    public class GetAdminDashboardQueryHandler(
       IUnitOfWork unitOfWork,
       ICacheService cacheService,
        ILogger<GetAdminDashboardQueryHandler> logger) : IRequestHandler<GetAdminDashboardQuery, ErrorOr<AdminDashboardDto>>
    {
        public async Task<ErrorOr<AdminDashboardDto>> Handle(GetAdminDashboardQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting get admin dashboard for user with id {UserId}", request.UserId);

            var cacheKey = $"AdminDashboard:{request.UserId}";
            var cachedDashboard = await cacheService.GetAsync<AdminDashboardDto>(cacheKey);
            if (cachedDashboard is not null)
            {
                logger.LogInformation("Get admin dashboard for user with id {UserId} returned from cache", request.UserId);
                return cachedDashboard;
            }

            logger.LogInformation("Getting admin dashboard data from database for user with id {UserId}", request.UserId);
            var dashboardDto = await unitOfWork.AdminDashboardRepository.GetAdminDashboardDataAsync();

            logger.LogInformation("Getting recent activities for user with id {UserId}", request.UserId);

            if (dashboardDto is null)
                return AdminDashboardErrors.GetDashboardDataFailed();

            await cacheService.SetAsync(cacheKey, dashboardDto, TimeSpan.FromMinutes(5));

            logger.LogInformation("Get admin dashboard for user with id {UserId} completed successfully", request.UserId);

            return dashboardDto;
        }
    }
}
