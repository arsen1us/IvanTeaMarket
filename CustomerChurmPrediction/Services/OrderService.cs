using CustomerChurmPrediction.Entities.OrderEntity;
using CustomerChurmPrediction.Entities.OrderEntity.Model;
using CustomerChurmPrediction.Entities.TeaEntity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface IOrderService : IBaseService<Order>
    {
        /// <summary>
        /// Загружает список заказов по id пользователя
        /// </summary>
        public Task<List<OrderModel>> GetByUserIdAsync(string userId, CancellationToken? cancellationToken = default);
    }

    /// <summary>
    /// Сервис для работы с заказами пользователей
    /// </summary>
    /// <param name="client"></param>
    /// <param name="config"></param>
    /// <param name="logger"></param>
    /// <param name="_environment"></param>
    /// <param name="_hubContext"></param>
    /// <param name="_userConnectionService"></param>
    public class OrderService(
        IMongoClient client,
        IConfiguration config,
        ILogger<OrderService> logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService) 
        : BaseService<Order>(client, config, logger, _environment, _hubContext, _userConnectionService, Orders), IOrderService
    {
        public async Task<List<OrderModel>> GetByUserIdAsync(string userId, CancellationToken? cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userId))
            {

                throw new ArgumentNullException();
            }

            var filter = Builders<Order>.Filter.Eq(order => order.UserId, userId);

            try
            {

                List<Order> userOrders = await FindAllAsync(filter, cancellationToken);

                List<string> teaIds = userOrders
                    .SelectMany(order => order.Items.Select(i => i.TeaId))
                    .Distinct()
                    .ToList();

                var productCollection = Database.GetCollection<Tea>(Teas);
                var teaFilter = Builders<Tea>.Filter.In(product => product.Id, teaIds);
                var productList = await (await productCollection.FindAsync(teaFilter)).ToListAsync();

                var productDict = productList.ToDictionary(p => p.Id);

                List<OrderModel> orderModels = userOrders.Select(order => new OrderModel
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    OrderStatus = order.OrderStatus,
                    TotalPrice = order.TotalPrice,
                    Items = order.Items.Select(item =>
                    {
                        var product = productDict.GetValueOrDefault(item.TeaId);
                        return new OrderItemModel
                        {
                            TeaId = item.TeaId,
                            teaName = product?.Name ?? "Неизвестный продукт",
                            ProductImageUrl = product?.ImageSrcs?.FirstOrDefault() ?? "",
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            TotalPrice = item.TotalPrice
                        };
                    }).ToList()
                }).ToList();

                return orderModels;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    
}
