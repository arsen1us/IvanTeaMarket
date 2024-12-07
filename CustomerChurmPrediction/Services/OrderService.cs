using CustomerChurmPrediction.Entities.OrderEntity;
using static CustomerChurmPrediction.Utils.CollectionName;
using MongoDB.Driver;
using CustomerChurmPrediction.Entities.ProductEntity;
using MongoDB.Driver.Linq;
using static CustomerChurmPrediction.Services.OrderService;

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

    public class OrderService(IMongoClient client, IConfiguration config, ILogger<OrderService> logger, IWebHostEnvironment _environment) 
        : BaseService<Order>(client, config, logger, _environment, Orders), IOrderService
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
                var _productCollection = Database.GetCollection<Product>(Products);

                var result = from order in Table.AsQueryable()
                             join product in _productCollection.AsQueryable() on order.ProductId equals product.Id
                             where order.UserId == userId
                             select new OrderModel { Order = order, Product = product };

                return await result.ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    
}
