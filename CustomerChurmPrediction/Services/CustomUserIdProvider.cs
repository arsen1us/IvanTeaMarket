using Microsoft.AspNetCore.SignalR;

namespace CustomerChurmPrediction.Services
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            // получить id из токена
            return connection.User?.FindFirst("Id")?.Value;
        }
    }
}
