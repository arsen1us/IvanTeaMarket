using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;
using CustomerChurmPrediction.Entities.UserEntity;

namespace CustomerChurmPrediction.Services
{
    public interface IUserService : IBaseService<User>
    {
        public Task<User> FindByEmailAndPassword(string email, string password, CancellationToken? cancellationToken = default);
    }

    // Сервис для работы с пользователями
    public class UserService(IMongoClient client, IConfiguration config, ILogger<UserService> logger) 
        : BaseService<User>(client, config, logger, Users), IUserService
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
