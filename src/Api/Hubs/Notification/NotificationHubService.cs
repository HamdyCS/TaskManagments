using Api.Hubs.Clients;
using Application.Common.Interfaces.Services;
using Application.Features.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs.Notification
{
    public class NotificationHubService(IHubContext<NotificationHub, NotificationHubClient> hubContext) : INotificationHubService
    {
        public async Task SendNotificationToUserAsync(string userId, NotificationDto notificationDto, CancellationToken cancellationToken = default)
        {
            await hubContext.Clients.User(userId).ReceiveNotification(notificationDto);
        }

        public Task SendNotificationToWorkSpaceUsersAsync(long workspaceId, NotificationDto notificationDto, CancellationToken cancellationToken = default)
        {
            return hubContext.Clients.Group($"workspace-{workspaceId}").ReceiveNotification(notificationDto);
        }
    }
}
