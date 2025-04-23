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
        IUserService _userService,
        ITeaService _teaService,
        ILogger<OrderController> _logger) : Controller
    {
        /// <summary>
        /// Получает спиcок заказов по id пользователя
        /// </summary>
        // GET: /api/order/user/{userId}

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
        
                var orders = await _orderService.GetByUserIdAsync(userId, cancellationToken);

                if (orders is null)
                {
                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByUserIdAsync)}] - Не удалось получить список заказов пользователя с id [{userId}]");
                    return NotFound();
                }

                _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(GetByUserIdAsync)}] - Успешно получен список заказов для пользователя с id [{userId}]. Количество записей: [{orders.Count}]");
                return Ok(new { orders = orders });
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByUserIdAsync)}] - Произошла ошибка в процессе получения заказов пользователя с id [{userId}]. Детали ошибки: {ex.Message}");
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

                List<Tea> teasForUpdate = new List<Tea>(createOrder.Items.Count);

                foreach (var createOrderItem in createOrder.Items)
                {
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
                        _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddOrderAsync)}] - Кол-во доступного чая меньше чем в заказе. Количество доступного чая - [{tea.Count}]. Количество в заказе -  [{createOrderItem.Quantity}]");
                        return BadRequest();
                    }

                    tea.Count -= createOrderItem.Quantity;

                    var orderItem = new OrderItem
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
                    teasForUpdate.Add(tea);
                }

                bool isSuccessTeasSave = await _teaService.SaveOrUpdateAsync(teasForUpdate, cancellationToken);

                if(!isSuccessTeasSave)
                {
                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddOrderAsync)}] - Не удалось сохранить список обновлённого чая. Число записей - [{teasForUpdate.Count}]");
                    return StatusCode(500, "Произошла ошибка во время сохранения списка обновлённого чая");
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
        /// Отменяет заказ
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
                var existingOrder = await _orderService.FindByIdAsync(orderId, cancellationToken);
                if (existingOrder is null)
                {
                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(CancelOrderAsync)}] - Не удалось найти заказ с id - [{orderId}]");
                    return NotFound();
                }

                List<Tea> teas = new List<Tea>(existingOrder.Items.Count);
                foreach (var item in existingOrder.Items)
                {
                    var tea = await _teaService.FindByIdAsync(item.TeaId);
                    if (tea is null)
                    {
                        _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(CancelOrderAsync)}] - Не удалось найти чай с id - [{item.TeaId}]");
                        return NotFound();
                    }
                    tea.Count += item.Quantity;
                    teas.Add(tea);
                }

                bool isSuccessTeasSave = await _teaService.SaveOrUpdateAsync(teas, cancellationToken);
                if(!isSuccessTeasSave)
                {
                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(CancelOrderAsync)}] - Не удалось сохранить заказ с обновлённой информацией о чая (отменённый заказ + возвращённое количество чая). Количество записей - [{teas.Count}]");
                    return StatusCode(500);
                }

                existingOrder.OrderStatus = "Canceled";
                bool isSuccessExistingOrderSave = await _orderService.SaveOrUpdateAsync(existingOrder, cancellationToken);
                if(!isSuccessExistingOrderSave)
                {
                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(CancelOrderAsync)}] - Не удалось сохранить заказ с обновлённой информацией (отменённый заказ). Id заказа - [{orderId}]");
                    return StatusCode(500);
                }

                return Ok(new { order = existingOrder });

            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(CancelOrderAsync)}] - Во время отмены заказа произошла ошибка. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
