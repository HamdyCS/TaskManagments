using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications.Command.GetAllUnReadUserNotifications
{
    public class GetAllUnReadUserNotificationsQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetAllUnReadUserNotificationsQueryHandler> logger) : IRequestHandler<GetAllUnReadUserNotificationsQuery, ErrorOr<PaginationResultDto<NotificationDto>>>
    {
        public async Task<ErrorOr<PaginationResultDto<NotificationDto>>> Handle(GetAllUnReadUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var pageNumber = request.PaginationRequestDto.PageNumber;
            var pageSize = request.PaginationRequestDto.PageSize;

            logger.LogInformation("Starting get notifications for user with id {UserId}", userId);

            var notifications = await unitOfWork.NotificationRepository
                .GetAllUnReadUserNotificationsAsync(userId, pageNumber, pageSize);

            logger.LogInformation("Get notifications for user with id {UserId} successfully",userId);

            return notifications.Adapt<PaginationResultDto<NotificationDto>>();
        }
    }
}
