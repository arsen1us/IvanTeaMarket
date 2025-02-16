using CustomerChurmPrediction.Entities.ProductEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.SignalR;

namespace CustomerChurmPrediction
{
    public class NotificationHub : Hub
    {
        private static readonly Dictionary<string, string> UserConnections = new();

        ICartService _cartService;
        IProductService _productService;
        IUserService _userService;

        public NotificationHub(ICartService cartService, IProductService productService, IUserService userService)
        {
            _cartService = cartService;
            _productService = productService;
            _userService = userService;
        }

        /// <summary>
        /// Обработка подключения к хабу
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            // Получить Id из Jwt-токена
            var userId = Context.UserIdentifier; 
            if (!string.IsNullOrEmpty(userId))
            {
                UserConnections[userId] = Context.ConnectionId;
                Console.WriteLine($"User {userId} connected with ConnectionId {Context.ConnectionId}");
            }

            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Обработка отключения от хаба
        /// </summary>
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                UserConnections.Remove(userId);
            }

            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Отправить всем уведомление
        /// </summary>
        public async Task SendNotificationForAll(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }

        /// <summary>
        /// Отправить уведомление о персональной скидке
        /// </summary>
        // userId - пользователь, кому надо отправить персональное уведомление
        public async Task SendPersonalDiscount(string message, string userId)
        {
            var user = await _userService.FindByIdAsync(userId, default);
            if (user is not null)
            {
                var connectionId = UserConnections.GetValueOrDefault(userId);
                if (!string.IsNullOrEmpty(connectionId))
                    await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
            }
        }

        /// <summary>
        /// Отправить уведомление "Вам может понравится"
        /// </summary>
        public async Task SendYouMightLikeNotification(string message, string userId, List<string> productIds)
        {
            var user = await _userService.FindByIdAsync(userId, default);
            if (user is not null)
            {
                var connectionId = UserConnections.GetValueOrDefault(userId);
                if (!string.IsNullOrEmpty(connectionId))
                    await Clients.Client(connectionId).SendAsync("ReceiveNotification", message, productIds);
            }
        }

        /// <summary>
        /// Отправить уведомление "Ранее вы добавили эти товары в корзину "
        /// </summary>
        public async Task SendYouPreviouslyAddedTheseItemsToYourCartNotification(string message, string userId, List<string> cartIds)
        {
            var user = await _userService.FindByIdAsync(userId, default);
            if (user is not null)
            {

                var connectionId = UserConnections.GetValueOrDefault(userId);
                if (!string.IsNullOrEmpty(connectionId))
                    await Clients.Client(connectionId).SendAsync("ReceiveNotification", message, cartIds);
            }
        }

        /// <summary>
        /// Отправить уведомление "Ранее вы смотрели эти товары"
        /// </summary>
        public async Task SendYouPreviouslyViewedTheseItemsNotification(string message, string userId, List<string> productIds)
        {
            var user = await _userService.FindByIdAsync(userId, default);
            if (user is not null)
            {
                var connectionId = UserConnections.GetValueOrDefault(userId);
                if (!string.IsNullOrEmpty(connectionId))
                    await Clients.Client(connectionId).SendAsync("ReceiveNotification", message, productIds);
            }
        }

        /// <summary>
        /// Отправить уведомление "Ранее вы смотрели эти товары"
        /// </summary>
        public async Task SendWeHaveGiftForYouNotification(string message, string userId)
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
    }

}
