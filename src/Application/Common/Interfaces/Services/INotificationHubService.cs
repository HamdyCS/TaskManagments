using Application.Features.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Services
{
    public interface INotificationHubService
    {
        public Task SendNotificationToUserAsync(string userId, NotificationDto notificationDto,CancellationToken cancellationToken = default);
        public Task SendNotificationToWorkSpaceUsersAsync(long workspaceId, NotificationDto notificationDto, CancellationToken cancellationToken = default);
    }
}
