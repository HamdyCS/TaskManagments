using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications.Command.CreateNotification
{
    public sealed record CreateNotificationCommand(CreateNotificationDto CreateNotificationDto) : IRequest<ErrorOr<NotificationDto>>;
   
}
