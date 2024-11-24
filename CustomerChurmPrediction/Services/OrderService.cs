using CustomerChurmPrediction.Entities.OrderEntity;
using static CustomerChurmPrediction.Utils.CollectionName;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Services
{
    public interface IOrderService : IBaseService<Order>
    {
        public Task<List<Order>> FindByCompanyIdAsync(string companyId, CancellationToken? cancellationToken = default);
    }

    public class OrderService(IMongoClient client, IConfiguration config, ILogger<OrderService> logger) 
        : BaseService<Order>(client, config, logger, Orders), IOrderService
    {
        public async Task<List<Order>> FindByCompanyIdAsync(string companyId, CancellationToken? cancellationToken = default)
        {
            if (string.IsNullOrEmpty(companyId))
                throw new ArgumentNullException();
            try
            {
                var filter = Builders<Order>.Filter.Eq(o => o.CompanyId, companyId);

                var orders = await base.FindAllAsync(filter, cancellationToken);
                if (orders is not null)
                    return orders;

                throw new Exception();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    
}
