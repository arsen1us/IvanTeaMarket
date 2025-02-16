using CustomerChurmPrediction.Entities.OrderEntity;
using CustomerChurmPrediction.Entities.OrderEntity.Create;
using CustomerChurmPrediction.Entities.ProductEntity;
using CustomerChurmPrediction.Entities.UserEntity;
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
        IProductService _productService) : Controller
    {
        /// <summary>
        /// Получить спиок заказов по id компании
        /// </summary>
        // GET: /api/order/company/{companyId}

        [Authorize(Roles = "Admin, Owner")]
        [HttpGet]
        [Route("company/{companyId}")]
        public async Task<IActionResult> GetByCompanyIdAsync(string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
                return BadRequest();
            try
            {
                var company = _companyService.FindByIdAsync(companyId, default);
        
                if(company is null)
                {
                    return NotFound();
                }
        
                var orderList = await _orderService.GetByCompanyIdAsync(companyId, default);
        
                if(orderList is null)
                {
                    return NotFound();
                }
                return Ok(new { orderList = orderList });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Получить спиcок заказов по id пользователя
        /// </summary>
        // GET: /api/order/company/{companyId}

        [Authorize(Roles = "User, Admin, Owner")]
        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IActionResult> GetByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest();
            try
            {
                var user = _userService.FindByIdAsync(userId, default);
        
                if (user is null)
                    return NotFound();
        
                var orderList = await _orderService.GetByUserIdAsync(userId, default);
        
                if (orderList is null)
                {
                    return NotFound();
                }
                return Ok(new { orderList = orderList });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Добавить новый заказ
        /// </summary>
        // POST: /api/order
        [HttpPost]
        public async Task<IActionResult> AddOrderAsync([FromBody]CreateOrderDto createOrder)
        {
            // Если объект createOrder равен null или он пуст
            if (createOrder is null|| !createOrder.Items.Any())
                return BadRequest("Переданные аргументы is null or empty");

            try
            {
                // Проверка, существует ли пользователь
                var user = await _userService.FindByIdAsync(createOrder.UserId);
                if (user is null)
                    return BadRequest("Пользователь с данным id не найден");

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

                    // Проверка, существует ли продукт
                    Product product = await _productService.FindByIdAsync(createOrderItem.ProductId, default);
                    if (product is null)
                        return BadRequest();

                    // Проверка количества продукта
                    if (product.Count < createOrderItem.Quantity)
                        return BadRequest();

                    orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = product.Id,
                        Quantity = createOrderItem.Quantity,
                        UnitPrice = product.Price,
                        TotalPrice = createOrderItem.Quantity * product.Price,
                        CreatorId = user.Id,
                        UserIdLastUpdate = user.Id,
                        CreateTime = DateTime.UtcNow,
                        LastTimeUserUpdate = DateTime.UtcNow
                    };

                    order.Items.Add(orderItem);
                }

                order.TotalPrice = order.Items.Sum(x => x.TotalPrice);

                bool isSuccess = await _orderService.SaveOrUpdateAsync(order, default);

                if (isSuccess)
                    return Ok(new { orderStatus = isSuccess });

                return StatusCode(500, "Произошла ошибка во время сохранения заказа. Деньги будут возвращены на счёт!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Удалить заказ по id
        /// </summary>
        // GET: /api/order/company/{companyId}

        [HttpDelete]
        [Route("{orderId}")]
        public async Task<IActionResult> CancelOrderAsync(string orderId)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
