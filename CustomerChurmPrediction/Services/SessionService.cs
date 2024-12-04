using CustomerChurmPrediction.Entities;
using CustomerChurmPrediction.Entities.ProductEntity;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface ISessionService : IBaseService<Session>
    {

    }
    /// <summary>
    /// Сервис для работы с сессиями
    /// </summary>
    public class SessionService(IMongoClient client, IConfiguration config, ILogger<SessionService> logger, IWebHostEnvironment _environment)
        : BaseService<Session>(client, config, logger, Sessions), ISessionService
    {

    }
}
