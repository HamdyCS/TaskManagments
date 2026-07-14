using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications.Command.GetNotificationByIdAndUserId
{
    public class GetNotificationByIdAndUserIdQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetNotificationByIdAndUserIdQueryHandler> logger) : IRequestHandler<GetNotificationByIdAndUserIdQuery, ErrorOr<NotificationDto>>
    {
        public async Task<ErrorOr<NotificationDto>> Handle(GetNotificationByIdAndUserIdQuery request, CancellationToken cancellationToken)
        {
            var notificationId = request.NotificationId;
            var userId = request.UserId;
           
            logger.LogInformation("Starting get notification with id {NotificationId} for user with id {UserId}", notificationId, userId);

            var notification = await unitOfWork.NotificationRepository
                .GetNotificationByIdAndUserIdAsync(notificationId, userId);

            if (notification is null)
            {
                logger.LogWarning("Notification with id {NotificationId} for user with id {UserId} not found", notificationId, userId);
                return NotificationErrors.NotificationNotFoundByIdAndUserId(notificationId, userId);
            }

            logger.LogInformation("get notification with id {NotificationId} for user with id {UserId} successfully", notificationId, userId);

            return notification.Adapt<NotificationDto>();
        }
    }
}
