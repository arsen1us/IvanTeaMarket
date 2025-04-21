using CustomerChurmPrediction.Entities;
using CustomerChurmPrediction.Entities.CartEntity;
using CustomerChurmPrediction.Entities.PageEntity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface INotificationService : IBaseService<Notification>
    {
        /// <summary>
        /// Получить последнии добавленные пользователем в корзину продукты
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<Cart>> GetLastAddedProductsToCartByUserIdAsync(string userId, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Получить последние посещённые пользователем страницы
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<Page>> GetLastPagesByUserIdAsync(string userId, CancellationToken? cancellationToken = default);
    }

    public class NotificationService(
        IMongoClient _client,
        IConfiguration _config, 
        ILogger<CartService> _logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService,
        IUserService _userService,
        ICartService _cartService,
        IPageService _pageService)
        : BaseService<Notification>(_client, _config, _logger, _environment, _hubContext, _userConnectionService, Notifications), INotificationService
    {
        public async Task<List<Cart>> GetLastAddedProductsToCartByUserIdAsync(string userId, CancellationToken? cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("");
                throw new ArgumentNullException("");
            }
            try
            {
                List<Cart> userCarts = await _cartService.FindByUserIdAsync(userId, cancellationToken);
                return userCarts;
            }
            catch (Exception ex)
            {
                _logger.LogError("");
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Page>> GetLastPagesByUserIdAsync(string userId, CancellationToken? cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("");
                throw new ArgumentNullException("");
            }
            try
            {
                List<Page> userPages = await _pageService.FindByUserIdAsync(userId, cancellationToken);
                return userPages;
            }
            catch (Exception ex)
            {
                _logger.LogError("");
                throw new Exception(ex.Message);
            }
        }
    }
}
