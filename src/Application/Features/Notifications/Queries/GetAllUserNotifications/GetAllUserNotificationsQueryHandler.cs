using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications.Command.GetAllUserNotifications
{
    public class GetAllUserNotificationsQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetAllUserNotificationsQueryHandler> logger) : IRequestHandler<GetAllUserNotificationsQuery, ErrorOr<PaginationResultDto<NotificationDto>>>
    {
        public async Task<ErrorOr<PaginationResultDto<NotificationDto>>> Handle(GetAllUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var pageNumber = request.PaginationRequestDto.PageNumber;
            var pageSize = request.PaginationRequestDto.PageSize;

            logger.LogInformation("Starting get unread notifications for user with id {UserId}", userId);

            var notifications = await unitOfWork.NotificationRepository
                .GetAllUnReadUserNotificationsAsync(userId, pageNumber, pageSize);

            logger.LogInformation("Get unread notifications for user with id {UserId} successfully",userId);

            return notifications.Adapt<PaginationResultDto<NotificationDto>>();
        }
    }
}
