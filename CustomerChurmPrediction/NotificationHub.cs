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
        /// Обработка подключения к хабу
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
        /// Обработка отключения от хаба
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
        /// Отправить уведомление при успешном действии в бд
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
        /// Отправить всем уведомление
        /// </summary>
        public async Task SendNotificationForAll(string message)
        {
            await Clients.All.SendAsync(SendForAll, message);
        }

        /// <summary>
        /// Отправить уведомление о персональной скидке
        /// </summary>
        // userId - пользователь, кому надо отправить персональное уведомление
        public async Task SendPersonalDiscount(string message, string userId)
        {
            try
            {
                // var user = await _userService.FindByIdAsync(userId, default);
                // if (user is not null)
                // {
                //     var connectionId = UserConnections.GetValueOrDefault(userId);
                //     if (!string.IsNullOrEmpty(connectionId))
                //         await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
                // }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Отправить уведомление "Вам может понравится"
        /// </summary>
        public async Task SendYouMightLikeNotification(string message, string userId, List<string> productIds)
        {
            try
            {
                // var user = await _userService.FindByIdAsync(userId, default);
                // if (user is not null)
                // {
                //     var connectionId = UserConnections.GetValueOrDefault(userId);
                //     if (!string.IsNullOrEmpty(connectionId))
                //         await Clients.Client(connectionId).SendAsync("ReceiveNotification", message, productIds);
                // }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Отправить уведомление "Ранее вы добавили эти товары в корзину "
        /// </summary>
        public async Task SendYouPreviouslyAddedTheseItemsToYourCartNotification(string message, string userId, List<string> cartIds)
        {
            try
            {
                // var user = await _userService.FindByIdAsync(userId, default);
                // if (user is not null)
                // {
                // 
                //     var connectionId = UserConnections.GetValueOrDefault(userId);
                //     if (!string.IsNullOrEmpty(connectionId))
                //         await Clients.Client(connectionId).SendAsync("ReceiveNotification", message, cartIds);
                // }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Отправить уведомление "Ранее вы смотрели эти товары"
        /// </summary>
        public async Task SendYouPreviouslyViewedTheseItemsNotification(string message, string userId, List<string> productIds)
        {
            try
            {
                // var user = await _userService.FindByIdAsync(userId, default);
                // if (user is not null)
                // {
                //     var connectionId = UserConnections.GetValueOrDefault(userId);
                //     if (!string.IsNullOrEmpty(connectionId))
                //         await Clients.Client(connectionId).SendAsync("ReceiveNotification", message, productIds);
                // }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Отправить уведомление "Ранее вы смотрели эти товары"
        /// </summary>
        public async Task SendWeHaveGiftForYouNotification(string message, string userId)
        {
            try
            {
                var user = await _userService.FindByIdAsync(userId, default);
                if (user is not null)
                {
                    // var cartList = await _cartService.FindProductsFromCartByUserId(userId, default);
                    // 
                    // 
                    // var connectionId = UserConnections.GetValueOrDefault(userId);
                    // if (!string.IsNullOrEmpty(connectionId))
                    //     await Clients.Client(connectionId).SendAsync("ReceiveNotification", message, cartList);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
