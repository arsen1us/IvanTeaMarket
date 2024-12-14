using CustomerChurmPrediction.Entities;
using CustomerChurmPrediction.Entities.CartEntity;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface INotificationService : IBaseService<Notification>
    {

    }

    public class NotificationService(IMongoClient client, IConfiguration config, ILogger<CartService> logger, IProductService _productService, IWebHostEnvironment _environment)
        : BaseService<Notification>(client, config, logger, _environment, Notifications), INotificationService
    {
        
    }
}
