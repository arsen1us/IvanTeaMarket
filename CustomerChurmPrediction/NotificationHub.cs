using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using static CustomerChurmPrediction.Utils.SignalRMethods;

namespace CustomerChurmPrediction
{
    public class NotificationHub(
        ICartService _cartService,
        IUserService _userService,
        IUserConnectionService _userConnectionService) : Hub
    {
        /// <summary>
        /// Обрабатывает подключение к хабу
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            try
            {
                // Получить Id из Jwt-токена
                var userId = Context.User?.FindFirst("Id").Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    _userConnectionService.AddConnection(userId, Context.ConnectionId);
                    await Clients.Caller.SendAsync(OnConnected, "Вы успешно подключились к сервису уведомлений!");
                }
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Обрабатывает отключение от хаба
        /// </summary>
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                _userConnectionService.RemoveConnection(userId);
            }

            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Отправляет уведомление при успешном действии в бд
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendDatabaseNotificationAsync(string message)
        {
            try
            {
                await Clients.Caller.SendAsync(SendDatabaseNotification, message);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Отправляет уведомление всем
        /// </summary>
        public async Task SendNotificationForAll(string message)
        {
            await Clients.All.SendAsync(SendForAll, message);
        }
    }

}
