using CustomerChurmPrediction.Entities.OrderEntity;
using CustomerChurmPrediction.Entities.OrderEntity.Create;
using CustomerChurmPrediction.Entities.TeaEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/order")]
    public class OrderController(
        IOrderService _orderService,
        ICompanyService _companyService,
        IUserService _userService,
        ITeaService _teaService,
        ILogger<OrderController> _logger) : Controller
    {
        /// <summary>
        /// Получает спиcок заказов по id пользователя
        /// </summary>
        // GET: /api/order/user/{userId}

        [Authorize(Roles = "User, Admin, Owner")]
        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IActionResult> GetByUserIdAsync(string userId)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByUserIdAsync)}] - Переданный параметр равен null или он пуст {nameof(userId)}");
                return BadRequest();
            }
            try
            {
                var user = await _userService.FindByIdAsync(userId, cancellationToken);
        
                if (user is null)
                {
                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByUserIdAsync)}] - Не удалось найти пользователя с id [{userId}]");
                    return NotFound();
                }
        
                var orderList = await _orderService.GetByUserIdAsync(userId, cancellationToken);

                if (orderList is null)
                {
                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByUserIdAsync)}] - Не удалось получить список заказов пользователя с id [{userId}]");
                    return NotFound();
                }

                _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(GetByUserIdAsync)}] - Успешно получен список заказов для пользователя с id [{userId}]. Кол {teas.Count}");
                return Ok(new { orderList = orderList });
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByUserIdAsync)}] - Успешно получен список чаёв. Число записей: {teas.Count}");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Создаёт заказ
        /// </summary>
        // POST: /api/order
        [HttpPost]
        public async Task<IActionResult> AddOrderAsync([FromBody]CreateOrderDto createOrder)
        {
            // Если объект createOrder равен null или он пуст
            if (createOrder is null|| !createOrder.Items.Any())
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddOrderAsync)}] - Не были передан параметр или он он пуст {nameof(createOrder)}");
                return BadRequest("Переданные аргументы is null or empty");
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;

            try
            {
                // Проверка, существует ли пользователь
                var user = await _userService.FindByIdAsync(createOrder.UserId, cancellationToken);
                if (user is null)
                {
                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddOrderAsync)}] - Не удалось найти пользователя с id [{createOrder.UserId}]");
                    return NotFound("Пользователь с данным id не найден");
                }

                Order order = new Order
                {
                    UserId = user.Id,
                    CreatorId = user.Id,
                    UserIdLastUpdate = user.Id,
                    OrderStatus = "Created",
                    CreateTime = DateTime.UtcNow,
                    LastTimeUserUpdate = DateTime.UtcNow,
                };

                foreach (var createOrderItem in createOrder.Items)
                {
                    OrderItem orderItem;

                    // Проверка, существует ли чай
                    Tea tea = await _teaService.FindByIdAsync(createOrderItem.teaId, cancellationToken);
                    if (tea is null)
                    {
                        _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddOrderAsync)}] - Не удалось получить чай с id [{createOrderItem.teaId}]");
                        return BadRequest();
                    }

                    // Проверка количества продукта
                    if (tea.Count < createOrderItem.Quantity)
                    {
                        _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddOrderAsync)}] - Успешно получен список чаёв. Число записей: {teas.Count}");
                        return BadRequest();
                    }


                    orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        TeaId = tea.Id,
                        Quantity = createOrderItem.Quantity,
                        UnitPrice = tea.Price,
                        TotalPrice = createOrderItem.Quantity * tea.Price,
                        CreatorId = user.Id,
                        UserIdLastUpdate = user.Id,
                        CreateTime = DateTime.UtcNow,
                        LastTimeUserUpdate = DateTime.UtcNow
                    };

                    order.Items.Add(orderItem);
                }

                order.TotalPrice = order.Items.Sum(x => x.TotalPrice);

                bool isSuccess = await _orderService.SaveOrUpdateAsync(order, cancellationToken);

                if (isSuccess)
                {
                    _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(AddOrderAsync)}] - Заказ с id [{order.Id}] успешно создан. Кол-во элементов заказа: [{order.Items.Count}]");
                    return Ok(new { order = order });
                }
                else
                {
                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddOrderAsync)}] - Не удалось сохранить заказ с id [{order.Id}]. Id заказчика [{order.UserId}]");
                    return StatusCode(500, "Произошла ошибка во время сохранения заказа. Деньги будут возвращены на счёт!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddOrderAsync)}] - Произошла ошибка во время создания заказа. Детали ошибки: {ex.Message}");
                throw new Exception($"[{DateTime.UtcNow} Method: {nameof(AddOrderAsync)}] - Произошла ошибка во время создания заказа. Детали ошибки: {ex.Message}");
            }
        }
        /// <summary>
        /// Уда
        /// </summary>
        // GET: /api/order/{orderId}

        [HttpDelete]
        [Route("{orderId}")]
        public async Task<IActionResult> CancelOrderAsync(string orderId)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;

            try
            {
                _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(CancelOrderAsync)}] - Успешно получен список чаёв. Число записей: {teas.Count}");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(CancelOrderAsync)}] - Успешно получен список чаёв. Число записей: {teas.Count}");
                throw new Exception(ex.Message);
            }
        }
    }
}
