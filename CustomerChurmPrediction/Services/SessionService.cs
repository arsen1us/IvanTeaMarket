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
    }

    public class SessionService(
        IMongoClient client,
        IConfiguration config,
        ILogger<SessionService> logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService)
        : BaseService<Session>(client, config, logger, _environment, _hubContext, _userConnectionService, Sessions), ISessionService
    {
        public async Task<Session> GetLastByUserIdAsync(string userId, CancellationToken? cancellationToken = default)
        {
            if(string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            try
            {
                var filter = Builders<Session>.Filter.Eq(s => s.UserId, userId);

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
    }
}
