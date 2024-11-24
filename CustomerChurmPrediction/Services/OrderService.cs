using CustomerChurmPrediction.Entities.OrderEntity;
using static CustomerChurmPrediction.Utils.CollectionName;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Services
{
    public interface IOrderService : IBaseService<Order>
    {

    }

    public class OrderService(IMongoClient client, IConfiguration config, ILogger<OrderService> logger) 
        : BaseService<Order>(client, config, logger, Orders), IOrderService
    {
        
    }

    
}
