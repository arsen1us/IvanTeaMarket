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
        public OrderController(IOrderService orderService, ICompanyService companyService) 
        {
            _companyService = companyService;
            _orderService = orderService;
        }

        /// <summary>
        /// Получить спиок заказов по id компании
        /// </summary>
        // GET: /api/order/company/{companyId}

        // [Authorize(Roles = "Admin, Owner")]
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

                var orderList = await _orderService.FindByCompanyIdAsync(companyId, default);

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
