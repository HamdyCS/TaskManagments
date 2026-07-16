using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications.Command.CreateNotification
{
    public class CreateNotificationCommandHandler(IUnitOfWork unitOfWork,
        ILogger<CreateNotificationCommandHandler> logger, INotificationHubService notificationHubService) : IRequestHandler<CreateNotificationCommand, ErrorOr<NotificationDto>>
    {
        public async Task<ErrorOr<NotificationDto>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var createNotificationDto = request.CreateNotificationDto;
            logger.LogInformation("Starting create notification for user with id {UserId}", createNotificationDto.NotifyToId);

            var notification = createNotificationDto.Adapt<Notification>();
            notification.CreatedAt = DateTime.UtcNow;
            notification.IsRead = false;

            logger.LogInformation("Creating notification for user with id {UserId}", createNotificationDto.NotifyToId);
            unitOfWork.NotificationRepository.Add(notification);

            var isAdded = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;
            if (!isAdded)
            {
                logger.LogWarning("Failed to create notification for user with id {UserId}", createNotificationDto.NotifyToId);
                return NotificationErrors.CreateNotificationFailed(createNotificationDto.NotifyToId);
            }

            //send notification to user
            var newNotificationDto = notification.Adapt<NotificationDto>();
            await notificationHubService.SendNotificationToUserAsync(notification.NotifyToId, newNotificationDto, cancellationToken);

            logger.LogInformation("Created notification for user with id {UserId} successfully", createNotificationDto.NotifyToId);
            return notification.Adapt<NotificationDto>();
        }
    }
}
