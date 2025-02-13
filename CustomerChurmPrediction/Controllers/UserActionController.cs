using CustomerChurmPrediction.Entities.UserActionEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("api/user-action")]
    public class UserActionController(IUserActionService _userActionService) : Controller
    {
        /// <summary>
        /// Добавить действие Добавить в корзину
        /// </summary>
        // POST: api/user-action/add-to-cart

        [HttpPost]
        [Route("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartAction userAction)
        {
            if (userAction is null)
                return BadRequest();
            try
            {
                bool isSuccess = await _userActionService.SaveOrUpdateAsync(userAction, default);
                if (isSuccess)
                    return Ok(true);

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Добавить действие Успешная попытка входа
        /// </summary>
        // POST: api/user-action/auth-seccess

        [HttpPost]
        [Route("auth-seccess")]
        public async Task<IActionResult> AuthenticationSuccess([FromBody] AuthenticateAction userAction)
        {
            if (userAction is null)
                return BadRequest();
            try
            {
                bool isSuccess = await _userActionService.SaveOrUpdateAsync(userAction, default);
                if (isSuccess)
                    return Ok(true);

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Добавить действие Купить продукт
        /// </summary>
        // POST: api/user-action/buy-product

        [HttpPost]
        [Route("buy-product")]
        public async Task<IActionResult> BuyProduct([FromBody] OrderAction userAction)
        {
            if (userAction is null)
                return BadRequest();
            try
            {
                bool isSuccess = await _userActionService.SaveOrUpdateAsync(userAction, default);
                if (isSuccess)
                    return Ok(true);

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Добавить действие Оставить отзыв
        /// </summary>
        // POST: api/user-action/create-review

        [HttpPost]
        [Route("create-review")]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewAction userAction)
        {
            if (userAction is null)
                return BadRequest();
            try
            {
                bool isSuccess = await _userActionService.SaveOrUpdateAsync(userAction, default);
                if (isSuccess)
                    return Ok(true);

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Добавить действие Открыть страницу
        /// </summary>
        // POST: api/user-action/open-page

        [HttpPost]
        [Route("open-page")]
        public async Task<IActionResult> OpenPage([FromBody] OpenPageAction userAction)
        {
            if (userAction is null)
                return BadRequest();
            try
            {
                bool isSuccess = await _userActionService.SaveOrUpdateAsync(userAction, default);
                if (isSuccess)
                    return Ok(true);

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Добавить действие Открыть рекламу
        /// </summary>
        // POST: api/user-action/reg-attempt

        [HttpPost]
        [Route("open-promotion")]
        public async Task<IActionResult> OpenPromotion([FromBody] OpenPromotionAction userAction)
        {
            if (userAction is null)
                return BadRequest();
            try
            {
                bool isSuccess = await _userActionService.SaveOrUpdateAsync(userAction, default);
                if (isSuccess)
                    return Ok(true);

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Добавить действие Попытка регистрации
        /// </summary>
        // POST: api/user-action/reg-attempt

        [HttpPost]
        [Route("reg-attempt")]
        public async Task<IActionResult> RegistrationAttempt([FromBody] RegisterAction userAction)
        {
            if (userAction is null)
                return BadRequest();
            try
            {
                bool isSuccess = await _userActionService.SaveOrUpdateAsync(userAction, default);
                if (isSuccess)
                    return Ok(true);

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Добавить действие Использовать купон
        /// </summary>
        // POST: api/user-action/use-coupon

        [HttpPost]
        [Route("use-coupon")]
        public async Task<IActionResult> UseCoupon([FromBody] UseCouponAction userAction)
        {
            if (userAction is null)
                return BadRequest();
            try
            {
                bool isSuccess = await _userActionService.SaveOrUpdateAsync(userAction, default);
                if (isSuccess)
                    return Ok(true);

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
