using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;
using CustomerChurmPrediction.Entities.CartEntity;
using CustomerChurmPrediction.Entities.UserEntity;
using CustomerChurmPrediction.Entities.ProductEntity;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;


namespace CustomerChurmPrediction.Services
{
    public interface ICartService : IBaseService<Cart>
    {
        /// <summary>
        /// Получить все товары в корзине по id пользователя
        /// </summary>
        public Task<List<Cart>> FindAllAsync(string userId, CancellationToken? cancellationToken = default);

        // <summary>
        /// Получить все продукты из корзины по id пользователя
        /// </summary>
        public Task<List<Product>> FindProductsFromCardByUserId(string userId, CancellationToken? cancellationToken = default);
    }

    public class CartService(
        IMongoClient client,
        IConfiguration config,
        ILogger<CartService> logger,
        IProductService _productService,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _notificationHubContext,
        IConnectionService _connectionService) 
        : BaseService<Cart>(client, config, logger, _environment, Carts, _notificationHubContext, _connectionService), ICartService
    {
        public async Task<List<Cart>> FindAllAsync(string userId, CancellationToken? cancellationToken = default)
        {
            var filter = Builders<Cart>.Filter.Eq(c => c.UserId, userId);
            var cartList = await base.FindAllAsync(filter, cancellationToken);

            return cartList;
        }

        public async Task<List<Product>> FindProductsFromCardByUserId(string userId, CancellationToken? cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException();
            try
            {
                var cartList = await FindAllAsync(userId, cancellationToken);

                var productIds = cartList.Select(cart => cart.ProductId).ToList();

                var productFilter = Builders<Product>.Filter.In("_id", productIds);

                var productList = await _productService.FindAllAsync(productFilter, cancellationToken);

                
                return productList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
