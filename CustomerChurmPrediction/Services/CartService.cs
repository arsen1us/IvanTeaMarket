using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;
using CustomerChurmPrediction.Entities.CartEntity;
using CustomerChurmPrediction.Entities.UserEntity;
using CustomerChurmPrediction.Entities.ProductEntity;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http.HttpResults;


namespace CustomerChurmPrediction.Services
{
    public interface ICartService : IBaseService<Cart>
    {
        /// <summary>
        /// Получить все товары в корзине по id пользователя
        /// </summary>
        public Task<List<Cart>> FindAllAsync(string userId, CancellationToken? cancellationToken = default);
    }

    public class CartService(IMongoClient client, IConfiguration config, ILogger<CartService> logger, IWebHostEnvironment _environment) 
        : BaseService<Cart>(client, config, logger, _environment, Carts), ICartService
    {
        public async Task<List<Cart>> FindAllAsync(string userId, CancellationToken? cancellationToken = default)
        {
            var filter = Builders<Cart>.Filter.Eq(c => c.UserId, userId);
            var cartList = await base.FindAllAsync(filter, cancellationToken);

            return cartList;
        }
    }
}
