using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;
using CustomerChurmPrediction.Entities.CartEntity;


namespace CustomerChurmPrediction.Services
{
    public interface ICartService : IBaseService<Cart>
    {
        public Task<List<Cart>> FindAllAsync(FilterDefinition<Cart>? filter, string userId);
    }

    // Сервис для работы с корзиной пользователей
    public class CartService(IMongoClient client, IConfiguration config, ILogger<CartService> logger) 
        : BaseService<Cart>(client, config, logger, Carts), ICartService
    {
        // Получить все товары в корзине по id пользователя
        public async Task<List<Cart>> FindAllAsync(FilterDefinition<Cart>? filter, string userId)
        {
            var resultFilter = filter ?? Builders<Cart>.Filter.Eq(c => c.UserId, userId);
            var result = await base.FindAllAsync(filter, default);

            return result;
        }
    }
}
