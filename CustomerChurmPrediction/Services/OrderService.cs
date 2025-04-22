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
        public Task<List<OrderDto>> GetByUserIdAsync(string userId, CancellationToken? cancellationToken = null);
    }

    /// <summary>
    /// Сервис для работы с заказами пользователей
    /// </summary>
    /// <param name="client"></param>
    /// <param name="config"></param>
    /// <param name="_logger"></param>
    /// <param name="_environment"></param>
    /// <param name="_hubContext"></param>
    /// <param name="_userConnectionService"></param>
    public class OrderService(
        IMongoClient client,
        IConfiguration config,
        ILogger<OrderService> _logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService, 
        ITeaService _teaService) 
        : BaseService<Order>(client, config, _logger, _environment, _hubContext, _userConnectionService, Orders), IOrderService
    {
        public async Task<List<OrderDto>> GetByUserIdAsync(string userId, CancellationToken? cancellationToken = null)
        {
            if (string.IsNullOrEmpty(userId))
            {

                throw new ArgumentNullException();
            }

            var filter = Builders<Order>.Filter.Eq(order => order.UserId, userId);

            try
            {
                List<Order> userOrders = await FindAllAsync(filter, cancellationToken);

                List<OrderDto> orderDtos = new List<OrderDto>(userOrders.Count); 

                foreach(var userOrder in userOrders)
                {
                    var userOrderTeaIds = userOrder.Items.Select(orderItem => orderItem.TeaId);
                    var teaFilter = Builders<Tea>.Filter.In(tea => tea.Id, userOrderTeaIds);
                    // Список чая из корзины пользователей
                    List<Tea> teas = await _teaService.FindAllAsync(teaFilter, cancellationToken);

                    OrderDto orderDto = new OrderDto
                    {
                        Order = userOrder,
                        Teas = teas
                    };

                    orderDtos.Add(orderDto);
                }

                return orderDtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    
}
