using CustomerChurmPrediction.Entities.OrderEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/order")]
    public class OrderController : Controller
    {
        IOrderService _orderService;
        ICompanyService _companyService;
        IUserService _userService;
        IProductService _productService;

        public OrderController(
            IOrderService orderService,
            ICompanyService companyService,
            IUserService userService,
            IProductService productService) 
        {
            _companyService = companyService;
            _orderService = orderService;
            _userService = userService;
            _productService = productService;
        }

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

                var orderList = await _orderService.GetOrderModelsByCompanyIdAsync(companyId, default);

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
        /// Получить спиок заказов по id пользователя
        /// </summary>
        // GET: /api/order/company/{companyId}

        [Authorize(Roles = "Admin, Owner")]
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

                var orderList = await _orderService.GetOrderModelsByUserIdAsync(userId, default);

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
        public async Task<IActionResult> AddOrderAsync([FromBody] OrderListDto orderAdd)
        {
            if (orderAdd is null
                || !orderAdd.OrderList.Any()
                || string.IsNullOrEmpty(orderAdd.UserId))
                return BadRequest("Переданные аргументы is null or empty");

            List<Order> orderList = new List<Order>();
            Order order;

            try
            {
                var user = await _userService.FindByIdAsync(orderAdd.UserId);
                if (user is null)
                    return BadRequest("Пользователь с данным id не найден");
                
                foreach(var item in orderAdd.OrderList)
                {
                    var product = await _productService.FindByIdAsync(item.ProductId, default);
                    // Продукт не найден
                    if (product is null)
                        return BadRequest();

                    // Количество товара на складе меньше чем в заказе
                    if (product.Count < item.Quantity)
                        return BadRequest();

                    order = new Order
                    {
                        UserId = user.Id,
                        CompanyId = product.CompanyId,
                        ProductId = product.Id,
                        ProductCount = item.Quantity,
                        CreateTime = DateTime.Now,
                        LastTimeUserUpdate = DateTime.Now,
                        TotalPrice = product.Price * item.Quantity,
                    };

                    if (order is not null)
                        orderList.Add(order);
                }

                if(orderList is not null)
                {
                    bool isSuccess = await _orderService.SaveOrUpdateAsync(orderList, default);

                    if (isSuccess)
                        return Ok(new { orderStatus = isSuccess });

                    return StatusCode(500, "Произошла ошибка во время сохранения заказа. Деньги будут возвращены на счёт!");
                }
                
                return StatusCode(500, "Произошла ошибка во время создания заказа. Деньги будут возвращены на счёт!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private decimal CalculateTotalOrderPrice(decimal pricePerOne, int count) => pricePerOne * count;
        
        /// <summary>
        /// Удалить заказ по id
        /// </summary>
        // GET: /api/order/company/{companyId}
        [HttpDelete]
        [Route("{companyId}")]
        public async Task<IActionResult> AddOrderAsync(string companyId)
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
