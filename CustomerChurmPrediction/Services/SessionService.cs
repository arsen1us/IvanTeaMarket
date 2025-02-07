using CustomerChurmPrediction.Entities.ProductEntity;
using CustomerChurmPrediction.Entities.SessionEntity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface ISessionService : IBaseService<Session>
    {
        /// <summary>
        /// Получить посследнию сессию по id пользователя
        /// </summary>
        public Task<Session> GetLastByUserIdAsync(string userId, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Получить сессию по id пользователя
        /// </summary>
        public Task<Session> FindByUserIdAsync(string userId, CancellationToken? cancellationToken = default);
    }

    public class SessionService(
        IMongoClient client,
        IConfiguration config,
        ILogger<SessionService> logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _notificationHubContext,
        IConnectionService connectionService)
        : BaseService<Session>(client, config, logger, _environment, Sessions, _notificationHubContext, connectionService), ISessionService
    {

        // Добавить функцию, которая автоматически будет добавлять к сессии время окончания (+30 минут)
        public async Task<Session> GetLastByUserIdAsync(string userId, CancellationToken? cancellationToken = default)
        {
            if(string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            try
            {
                
                var filter = Builders<Session>.Filter.Eq(s => s.UserId, userId);

                // Сортирую по времени начала, так как время окончания nullабельно
                var sort = Builders<Session>.Sort.Descending(s => s.SessionTimeStart);

                var session = await Table.Find(filter)
                                         .Sort(sort)
                                         .Limit(1)
                                         .FirstOrDefaultAsync();

                // Если период между запросами меньше 60 минут, то обновляю сессию. Иначе - создаю новую
                var period = DateTime.Now - session.SessionTimeStart;
                if(period.TotalMinutes <= 60)
                {
                    //обновление сессии
                    session.SessionTimeEnd = DateTime.Now;

                    await SaveOrUpdateAsync(session, default);
                }
                else
                {
                    // Если у прошлой сессии, отсутствует время окончания, то до его
                    if(session.SessionTimeEnd is null)
                    {
                        session.SessionTimeEnd = DateTime.Now;
                    }
                    // создание новой
                    Session newSession = new Session
                    {
                        UserId = session.UserId,
                        CreatorId = session.CreatorId,
                        UserIdLastUpdate = session.UserIdLastUpdate,
                        CreateTime = DateTime.Now,
                        SessionTimeStart = DateTime.Now,
                    };

                    List<Session> sessions = new List<Session>
                    {
                        session,
                        newSession
                    };

                    bool isSuccess = await SaveOrUpdateAsync(sessions, default);
                }

                var sessionList = await FindAllAsync(filter, default);

                // Так то это должно выдавать сессию, созданную самой последней
                var lastSession = sessionList.LastOrDefault();
                return lastSession;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Session> FindByUserIdAsync(string userId, CancellationToken? cancellationToken = default)
        {
            return new Session();
        }
    }
}
