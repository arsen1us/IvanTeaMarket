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
    }

    public class CartService(
        IMongoClient client,
        IConfiguration config,
        ILogger<CartService> logger, 
        IWebHostEnvironment _environment, 
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService) 
        : BaseService<Cart>(client, config, logger, _environment, _hubContext, _userConnectionService, Carts), ICartService
    {
        public async Task<List<Cart>> FindAllAsync(string userId, CancellationToken? cancellationToken = default)
        {
            var filter = Builders<Cart>.Filter.Eq(c => c.UserId, userId);
            var cartList = await base.FindAllAsync(filter, cancellationToken);

            return cartList;
        }
    }
}
