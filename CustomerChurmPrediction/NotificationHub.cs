using Microsoft.AspNetCore.SignalR;

namespace CustomerChurmPrediction
{
    public class NotificationHub : Hub
    {
        /// <summary>
        /// Отправить всем уведомление
        /// </summary>
        public async Task SendNotificationForAll(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }

}
