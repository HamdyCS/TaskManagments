using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications.Command.ReadNotification
{
    public class MarkNotificationAsReadHandler(IUnitOfWork unitOfWork,
        ILogger<MarkNotificationAsReadHandler> logger) : IRequestHandler<MarkNotificationAsReadCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            var notificationId = request.NotificationId;
            var notifyToId = request.NotifyToId;

            logger.LogInformation("Starting read notification with id {NotificationId} for user with id {UserId}", notificationId, notifyToId);

            var notification = await unitOfWork.NotificationRepository.GetNotificationByIdAndUserIdAsync(notificationId,notifyToId);
            if(notification is null)
            {
                logger.LogWarning("Notification with id {NotificationId} not found for user with id {UserId}", notificationId, notifyToId);
                return NotificationErrors.NotificationNotFoundByIdAndUserId(notificationId, notifyToId);
            }

            //check if notification is already read
            if (notification.IsRead)
            {
                logger.LogWarning("Notification with id {NotificationId} is already read for user with id {UserId}", notificationId, notifyToId);
                return NotificationErrors.NotificationAlreadyRead(notificationId, notifyToId);
            }


            //update notification as read
            logger.LogInformation("Reading notification with id {NotificationId} for user with id {UserId}", notificationId, notifyToId);

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            unitOfWork.NotificationRepository.Update(notification);

            var isUpdated = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;
            if (!isUpdated)
            {
                logger.LogWarning("Failed to create notification with id {NotificationId} for user with id {UserId}", notificationId, notifyToId);
                return NotificationErrors.UpdateNotificationToReadFailed(notificationId, notifyToId);
            }

            logger.LogInformation("Notification with id {NotificationId} for user with id {UserId} has been read successfully", notificationId, notifyToId);
            return true;
        }
    }
}
