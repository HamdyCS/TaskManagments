using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications.Command.ReadNotification
{
    public sealed record MarkNotificationAsReadCommand(long NotificationId,string NotifyToId) : IRequest<ErrorOr<bool>>;
   
}
