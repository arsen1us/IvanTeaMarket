using MongoDB.Driver;
using CustomerChurmPrediction.Entities;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface IFavoriteService : IBaseService<Favorite>
    {
        public Task<List<Favorite>> FindAllAsync(FilterDefinition<Favorite>? filter, string userId);
    }

    // Сервис для работы со вкладкой избранное 
    public class FavoriteService(IMongoClient client, IConfiguration config, ILogger<FavoriteService> logger) 
        : BaseService<Favorite>(client, config, logger, Favorites), IFavoriteService
    {
        // Получить все товары в списке избранного по id пользователя
        public async Task<List<Favorite>> FindAllAsync(FilterDefinition<Favorite>? filter, string userId)
        {
            var resultFilter = filter ?? Builders<Favorite>.Filter.Eq(c => c.UserId, userId);
            var result = await base.FindAllAsync(filter, default);

            return result;
        }
    }
}
