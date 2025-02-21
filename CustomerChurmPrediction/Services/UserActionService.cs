using CustomerChurmPrediction.Entities.UserActionEntity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface IUserActionService : IBaseService<UserAction>
    {

    }

    public class UserActionService(
        IMongoClient client,
        IConfiguration config,
        ILogger<UserActionService> logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService)
        : BaseService<UserAction>(client, config, logger, _environment, _hubContext, _userConnectionService, UserActions), IUserActionService
    {

        /// <summary>
        /// Получить по id пользователя
        /// </summary>
        public async Task<List<UserAction>> GetByUserId(string userId, CancellationToken? cancellationToken = default)
        {
            if(string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            try
            {
                var filter = Builders<UserAction>.Filter.Eq(a => a.UserId, userId);

                var userActionList = await base.FindAllAsync(filter, cancellationToken);

                return userActionList;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
