using Api.Hubs.Clients;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs.Notification
{
    public sealed class NotificationHub : Hub<NotificationHubClient>
    {
        public async Task JoinWorkSpace(long workSpaceId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"workspace-${workSpaceId}");
        }

        public async Task LeaveWorkSpace(long workSpaceId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"workspace-${workSpaceId}");
        }
    }
   
}
