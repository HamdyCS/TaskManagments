using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications
{
    public record NotificationDto(long Id, string NotifyToId, long? TaskId, long? WorkSpaceInviteId,
        string Title, string Message, DateTime CreatedAt, bool IsRead, DateTime? ReadAt, NotificationType NotificationType);
}
