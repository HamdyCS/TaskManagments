using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications.Command.CreateNotification
{
    public record CreateNotificationDto(string NotifyToId, long? TaskId, long? WorkSpaceInviteId, 
        string Title, string Message, NotificationType NotificationType);
}
