using CustomerChurmPrediction.Entities;
using CustomerChurmPrediction.Entities.CartEntity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface INotificationService : IBaseService<Notification>
    {

    }

    public class NotificationService(
        IMongoClient client,
        IConfiguration config, 
        ILogger<CartService> logger,
        IProductService _productService,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService)
        : BaseService<Notification>(client, config, logger, _environment, _hubContext, _userConnectionService, Notifications), INotificationService
    {
        
    }
}
