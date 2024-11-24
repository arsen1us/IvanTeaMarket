using CustomerChurmPrediction.Entities.OrderEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/order")]
    public class OrderController : Controller
    {
        IOrderService _orderService;
        public OrderController(IOrderService orderService) 
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Получить спиок заказов по id компании
        /// </summary>
        /// <param name="companyId">Id компании</param>
        // GET: /api/order/company/{companyId}
        [HttpGet]
        [Route("company/{companyId}")]
        public async Task<IActionResult> GetOrderListByCompanyIdAsync(string companyId)
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

        /// <summary>
        /// Добавить новый заказ
        /// </summary>
        /// <param name="orderAdd">Новый заказ</param>
        // POST: /api/order
        [HttpPost]
        public async Task<IActionResult> AddOrderAsync([FromBody] OrderAdd orderAdd)
        {
            if (orderAdd is null)
                return BadRequest();
            try
            {

                Order order = new Order
                {
                    ProductId = orderAdd.ProductId,
                    ProductCount = orderAdd.ProductCount,
                    CompanyId = orderAdd.CompanyId,
                    UserId = orderAdd.UserId,
                    TotalPrice = CalculateTotalOrderPrice(orderAdd.Price, orderAdd.ProductCount)
                };

                if(order is not null)
                {
                    bool isSuccess = await _orderService.SaveOrUpdateAsync(order, default);
                    if(isSuccess)
                        return Ok(new {orderStatus = isSuccess});

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
        /// <param name="companyId">Id компании</param>
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
