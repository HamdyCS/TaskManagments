using Application.Common.Dtos.Dashboard;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Features.Notifications.Command.GetAllUnReadUserNotifications;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Dashboard.Queries.GetDashboard
{
    public class GetDashboardQueryHandler(IUnitOfWork unitOfWork,
        IMediator mediator,ICacheService cacheService,
        ILogger<GetDashboardQueryHandler> logger) : IRequestHandler<GetDashboardQuery, ErrorOr<DashboardDto>>
    {
        public async Task<ErrorOr<DashboardDto>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var workspaceId = request.WorkSpaceId;
            var specificallyForThisUser = request.SpecificallyForThisUser;
            DashboardDto dashboardDto;

            logger.LogInformation("Starting get workspace user dashboard for user with id {UserId} in workspace with id {WorkSpaceId}", userId, workspaceId);

            logger.LogInformation("Getting workspace dashboard for user with id {UserId} in workspace with id {WorkSpaceId}", userId, workspaceId);

            //check if workspace dashboard is cached for user
            var cacheKey = $"WorkSpaceDashboard:{workspaceId}:{userId}";
            var workSpaceDashboardCached = await cacheService.GetAsync<DashboardDto>(cacheKey);
            if (workSpaceDashboardCached != null)
            {
                dashboardDto = workSpaceDashboardCached;
            }
            else
            {
                //get workspace dashboard for user
                dashboardDto =
                    specificallyForThisUser ?
                    await unitOfWork.DashboardRepository.GetWorkSpaceDashboardByUserIdAsync(workspaceId, userId) :
                    await unitOfWork.DashboardRepository.GetWorkSpaceDashboardAsync(workspaceId);
                //cache workspace dashboard for user
                await cacheService.SetAsync(cacheKey, dashboardDto, TimeSpan.FromMinutes(5));
            }

            
            //get unread notifications for user in workspace
            var unReadNotificationResult = await mediator.Send(new GetAllUnReadUserNotificationsQuery(userId,
                new Common.Dtos.PaginationRequestDto { PageNumber = 1, PageSize = 10 }));


            dashboardDto.UnReadNotifications = unReadNotificationResult.Value.Data;


            logger.LogInformation("Get workspace user dashboard for user with id {UserId} in workspace with id {WorkSpaceId} successfully", userId, workspaceId);

            return dashboardDto;

        }
    }
}