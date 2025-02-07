using CustomerChurmPrediction.Entities.OrderEntity;
using static CustomerChurmPrediction.Utils.CollectionName;
using MongoDB.Driver;
using CustomerChurmPrediction.Entities.ProductEntity;
using MongoDB.Driver.Linq;
using static CustomerChurmPrediction.Services.OrderService;
using Microsoft.AspNetCore.SignalR;

namespace CustomerChurmPrediction.Services
{
    public interface IOrderService : IBaseService<Order>
    {
        /// <summary>
        /// Получить список заказов по id компаниии
        /// </summary>
        public Task<List<OrderModel>> GetOrderModelsByCompanyIdAsync(string companyId, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Получить список заказов по id пользователя
        /// </summary>
        public Task<List<OrderModel>> GetOrderModelsByUserIdAsync(string userId, CancellationToken? cancellationToken = default);
    }

    public class OrderService(
        IMongoClient client,
        IConfiguration config,
        ILogger<OrderService> logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _notificationHubContext,
        IConnectionService _connectionService) 
        : BaseService<Order>(client, config, logger, _environment, Orders, _notificationHubContext, _connectionService), IOrderService
    {
        public async Task<List<OrderModel>> GetOrderModelsByCompanyIdAsync(string companyId, CancellationToken? cancellationToken = default)
        {
            if (string.IsNullOrEmpty(companyId))
                throw new ArgumentNullException();
            try
            {
                var _productCollection = Database.GetCollection<Product>(Products);

                var result = from order in Table.AsQueryable()
                             join product in _productCollection.AsQueryable() on order.ProductId equals product.Id
                             where order.CompanyId == companyId
                             select new OrderModel { Order = order, Product = product };

                return await result.ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<List<OrderModel>> GetOrderModelsByUserIdAsync(string userId, CancellationToken? cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException();
            try
            {
                string connectionId = _connectionService.GetConnectionIdByUserId(userId);

                var _productCollection = Database.GetCollection<Product>(Products);

                var result = from order in Table.AsQueryable()
                             join product in _productCollection.AsQueryable() on order.ProductId equals product.Id
                             where order.UserId == userId
                             select new OrderModel { Order = order, Product = product };

                await _notificationHubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", $"Успешно получен списко отзывов");
                return await result.ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task<long> DeleteAsync(string entityId, CancellationToken? cancellationToken = default)
        {
            try
            {
                if (entityId != null)
                {
                    var filter = Builders<Order>.Filter.Eq(e => e.Id, entityId);
                    var result = await Table.DeleteOneAsync(filter);
                    if (result.DeletedCount > 0)
                        return result.DeletedCount;
                    return 0;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
    }

    
}
