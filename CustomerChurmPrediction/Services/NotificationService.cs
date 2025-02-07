using CustomerChurmPrediction.Entities.CartEntity;
using CustomerChurmPrediction.Entities.NotificationEntity;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface INotificationService : IBaseService<Notification>
    {
        /// <summary>
        /// Получить список уведомлений по id пользователя 
        /// </summary>
        public Task<List<Notification>> FindByUserIdAsync(string userId, CancellationToken? cancellationToken = default);
    }

    public class NotificationService(IMongoClient client, IConfiguration config, ILogger<CartService> logger, IProductService _productService, IWebHostEnvironment _environment)
        : BaseService<Notification>(client, config, logger, _environment, Notifications), INotificationService
    {
        public async Task<List<Notification>> FindByUserIdAsync(string userId, CancellationToken? cancellationToken = default)
        {
            try
            {
                var filter = Builders<Notification>.Filter.Eq(n => n.UserId, userId);

                var notificationList = await FindAllAsync(filter, cancellationToken);

                return notificationList;
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла ошибка при получении уведомлений по id пользователя. Детали:" + ex.Message);
            }
        }
    }
}
