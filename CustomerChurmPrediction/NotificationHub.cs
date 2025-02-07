using CustomerChurmPrediction.Entities.ProductEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.SignalR;

namespace CustomerChurmPrediction
{
    public class NotificationHub : Hub
    {
        ICartService _cartService;
        IProductService _productService;
        IUserService _userService;
        IConnectionService _connectionService;
        IPersonalNotificationService _personalNotificationService;

        public NotificationHub(
            ICartService cartService,
            IProductService productService,
            IUserService userService,
            IConnectionService connectionService,
            IPersonalNotificationService personalNotificationService)
        {
            _cartService = cartService;
            _productService = productService;
            _userService = userService;
            _connectionService = connectionService;
            _personalNotificationService = personalNotificationService;
        }

        /// <summary>
        /// Обработка подключения к хабу
        /// </summary>
        public override Task OnConnectedAsync()
        {
            try
            {
                var userId = Context.UserIdentifier;
                if (!string.IsNullOrEmpty(userId))
                    _connectionService.AddConnection(userId, Context.ConnectionId);

                return base.OnConnectedAsync();
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
            try
            {
                var userId = Context.UserIdentifier;
                if (!string.IsNullOrEmpty(userId))
                    _connectionService.RemoveConnection(Context.UserIdentifier);

                return base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Отправить уведомление все пользователям
        /// </summary>
        public async Task SendNotificationForAll(string message)
        {
            try
            {
                await Clients.All.SendAsync("ReceiveNotification", message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Отправить пользователю персонализированное уведомление со списком продуктов
        /// </summary>
        public async Task SendPersonalNotificationAsync(string userId)
        {
            try
            {
                var user = await _userService.FindByIdAsync(userId, default);
                if (user is null)
                {
                    List<string> productIds = await _personalNotificationService.GetPersonalProductListByUserId(userId, default);

                    var connectionId = _connectionService.GetConnectionIdByUserId(userId);
                    if (!string.IsNullOrEmpty(connectionId))
                    {
                        if (!string.IsNullOrEmpty(connectionId))
                            await Clients.Client(connectionId).SendAsync("ReceivePersonalNotification", "Ранее вы интересовались данными продуктами: ", productIds);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    


        /// <summary>
        /// Отправить уведомление о персональной скидке
        /// </summary>
        // userId - пользователь, кому надо отправить персональное уведомление
        public async Task SendPersonalDiscount(string message, string userId)
        {
            // var user = await _userService.FindByIdAsync(userId, default);
            // if (user is not null)
            // {
            //     var connectionId = UserConnections.GetValueOrDefault(userId);
            //     if (!string.IsNullOrEmpty(connectionId))
            //         await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
            // }
        }

        /// <summary>
        /// Отправить уведомление "Вам может понравится"
        /// </summary>
        public async Task SendYouMightLikeNotification(string message, string userId, List<string> productIds)
        {
            // var user = await _userService.FindByIdAsync(userId, default);
            // if (user is not null)
            // {
            //     var connectionId = UserConnections.GetValueOrDefault(userId);
            //     if (!string.IsNullOrEmpty(connectionId))
            //         await Clients.Client(connectionId).SendAsync("ReceiveNotification", message, productIds);
            // }
        }

        /// <summary>
        /// Отправить уведомление "Ранее вы добавили эти товары в корзину "
        /// </summary>
        public async Task SendYouPreviouslyAddedTheseItemsToYourCartNotification(string message, string userId, List<string> cartIds)
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

        /// <summary>
        /// Отправить уведомление "Ранее вы смотрели эти товары"
        /// </summary>
        public async Task SendYouPreviouslyViewedTheseItemsNotification(string message, string userId, List<string> productIds)
        {
            // var user = await _userService.FindByIdAsync(userId, default);
            // if (user is not null)
            // {
            //     var connectionId = UserConnections.GetValueOrDefault(userId);
            //     if (!string.IsNullOrEmpty(connectionId))
            //         await Clients.Client(connectionId).SendAsync("ReceiveNotification", message, productIds);
            // }
        }

        /// <summary>
        /// Отправить уведомление "Ранее вы смотрели эти товары"
        /// </summary>
        public async Task SendWeHaveGiftForYouNotification(string message, string userId)
        {
            // var user = await _userService.FindByIdAsync(userId, default);
            // if (user is not null)
            // {
            //     var cartList = await _cartService.FindProductsFromCardByUserId(userId, default);
            // 
            // 
            //     var connectionId = UserConnections.GetValueOrDefault(userId);
            //     if (!string.IsNullOrEmpty(connectionId))
            //         await Clients.Client(connectionId).SendAsync("ReceiveNotification", message, cartList);
            // }
        }
    }

}
