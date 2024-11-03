using MongoDB.Driver;
using CustomerChurmPrediction.Entities;
using static CustomerChurmPrediction.Utils.CollectionName;


namespace CustomerChurmPrediction.Services
{
    public interface ICartService : IBaseService<Cart>
    {

    }

    // Сервис для работы с корзиной пользователей
    public class CartService(IMongoClient client, IConfiguration config) : BaseService<Cart>(client, config, Carts), ICartService
    {

    }
}
