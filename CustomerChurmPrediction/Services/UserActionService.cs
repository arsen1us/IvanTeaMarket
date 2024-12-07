using CustomerChurmPrediction.Entities.UserActionEntity;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface IUserActionService : IBaseService<UserAction>
    {

    }

    public class UserActionService(IMongoClient client, IConfiguration config, ILogger<UserActionService> logger, IWebHostEnvironment _environment)
        : BaseService<UserAction>(client, config, logger, _environment, UserActions), IUserActionService
    {

    }
}
