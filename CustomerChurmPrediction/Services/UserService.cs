using MongoDB.Driver;
using CustomerChurmPrediction.Entities;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface IUserService : IBaseService<User>
    {

    }

    // Сервис для работы с пользователями
    public class UserService(IMongoClient client, IConfiguration config) : BaseService<User>(client, config, Users), IUserService
    {

    }
}
