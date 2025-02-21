using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;
using CustomerChurmPrediction.Entities.UserEntity;
using Microsoft.AspNetCore.SignalR;

namespace CustomerChurmPrediction.Services
{
    public interface IUserService : IBaseService<User>
    {
        /// <summary>
        /// Получить пользователя по почте и паролю
        /// </summary>
        public Task<User> FindByEmailAndPassword(string email, string password, CancellationToken? cancellationToken = default);
    }

    public class UserService(
        IMongoClient client,
        IConfiguration config,
        ILogger<UserService> logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService) 
        : BaseService<User>(client, config, logger, _environment, _hubContext, _userConnectionService, Users), IUserService
    {
        public async Task<User> FindByEmailAndPassword(string email, string password, CancellationToken? cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    throw new ArgumentNullException();
                }
                var filter = Builders<User>.Filter.And(
                        Builders<User>.Filter.Eq(u => u.Email, email),
                        Builders<User>.Filter.Eq(u => u.Password, password));

                var user = (await base.FindAllAsync(filter, cancellationToken)).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
