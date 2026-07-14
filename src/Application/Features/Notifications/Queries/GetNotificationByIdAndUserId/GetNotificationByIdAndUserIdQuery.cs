using Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications.Command.GetNotificationByIdAndUserId
{
    public sealed record GetNotificationByIdAndUserIdQuery(long NotificationId, string UserId) : IRequest<ErrorOr<NotificationDto>>;
   
}
