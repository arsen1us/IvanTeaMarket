using MongoDB.Driver;
using CustomerChurmPrediction.Entities;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface IUserService : IBaseService<User>
    {

    }

    // Сервис для работы с пользователями
    public class UserService(IMongoClient client, IConfiguration config, ILogger<UserService> logger) 
        : BaseService<User>(client, config, logger, Users), IUserService
    {

    }
}
