using MongoDB.Driver;
using CustomerChurmPrediction.Entities;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface IFavoriteService : IBaseService<Favorite>
    {

    }

    // Сервис для работы со вкладкой избранное 
    public class FavoriteService(IMongoClient client, IConfiguration config) : BaseService<Favorite>(client, config, Favorites), IFavoriteService
    {
    }
}
