using Application.Features.Notifications;

namespace Api.Hubs.Clients
{
    public interface NotificationHubClient
    {
        Task ReceiveNotification(NotificationDto notificationDto);
    }
}
