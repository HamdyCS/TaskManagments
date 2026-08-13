using Application.Common.Dtos.WorkSpaceUserDashboard;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Features.Notifications.Command.GetAllUnReadUserNotifications;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceUserDashboard.Queries.GetWorkSpaceUserDashboard
{
    public class GetWorkSpaceUserDashboardQueryHandler(IUnitOfWork unitOfWork,
        IMediator mediator,ICacheService cacheService,
        ILogger<GetWorkSpaceUserDashboardQueryHandler> logger) : IRequestHandler<GetWorkSpaceUserDashboardQuery, ErrorOr<WorkSpaceDashboardDto>>
    {
        public async Task<ErrorOr<WorkSpaceDashboardDto>> Handle(GetWorkSpaceUserDashboardQuery request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var workspaceId = request.WorkSpaceId;
            var specificallyForThisUser = request.SpecificallyForThisUser;
            WorkSpaceDashboardDto workSpaceDashboardDto;

            logger.LogInformation("Starting get workspace user dashboard for user with id {UserId} in workspace with id {WorkSpaceId}", userId, workspaceId);

            logger.LogInformation("Getting workspace dashboard for user with id {UserId} in workspace with id {WorkSpaceId}", userId, workspaceId);

            //check if workspace dashboard is cached for user
            var cacheKey = $"WorkSpaceDashboard:{workspaceId}:{userId}";
            var workSpaceDashboardCached = await cacheService.GetAsync<WorkSpaceDashboardDto>(cacheKey);
            if (workSpaceDashboardCached != null)
            {
                workSpaceDashboardDto = workSpaceDashboardCached;
            }
            else
            {
                //get workspace dashboard for user
                workSpaceDashboardDto =
                    specificallyForThisUser ?
                    await unitOfWork.WorkSpaceDashboardRepository.GetWorkSpaceDashboardByUserIdAsync(workspaceId, userId) :
                    await unitOfWork.WorkSpaceDashboardRepository.GetWorkSpaceDashboardAsync(workspaceId);
                //cache workspace dashboard for user
                await cacheService.SetAsync(cacheKey, workSpaceDashboardDto, TimeSpan.FromMinutes(5));
            }

            
            //get unread notifications for user in workspace
            var unReadNotificationResult = await mediator.Send(new GetAllUnReadUserNotificationsQuery(userId,
                new Common.Dtos.PaginationRequestDto { PageNumber = 1, PageSize = 10 }));


            workSpaceDashboardDto.UnReadNotifications = unReadNotificationResult.Value.Data;


            logger.LogInformation("Get workspace user dashboard for user with id {UserId} in workspace with id {WorkSpaceId} successfully", userId, workspaceId);

            return workSpaceDashboardDto;

        }
    }
}