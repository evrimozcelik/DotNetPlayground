using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SampleRabbitMQData;

namespace SampleSignalRHub.SignalR
{
    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return Clients.Client(Context.ConnectionId).SendAsync("SetConnectionId", Context.ConnectionId);
        }

        public async Task<string> ConnectGroup(string notificationName, string connectionID)
        {
            await Groups.AddToGroupAsync(connectionID, notificationName);
            return $"{connectionID} is added {notificationName}";
        }

        public Task PushNotify(Notification notification)
        {
            return Clients.Group(notification.Name).SendAsync("ChangeValue", notification);
        }


    }
}
