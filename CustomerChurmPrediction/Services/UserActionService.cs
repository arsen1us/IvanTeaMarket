using CustomerChurmPrediction.Entities.UserActionEntity;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface IUserActionService : IBaseService<UserAction>
    {

    }

    /// <summary>
    /// Сервис для работы с действиями пользователей
    /// </summary>
    public class UserActionService(IMongoClient client, IConfiguration config, ILogger<UserActionService> logger, IWebHostEnvironment _environment)
        : BaseService<UserAction>(client, config, logger, UserActions), IUserActionService
    {

    }
}
