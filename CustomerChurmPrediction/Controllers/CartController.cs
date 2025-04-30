using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Entities.CartEntity;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using CustomerChurmPrediction.Entities.TeaEntity;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/cart")]
    public class CartController(
        ICartService _cartService,
        IUserService _userService,
        ITeaService _teaService,
        ILogger<CartController> _logger) : ControllerBase
    {
        /// <summary>
        /// Загружает корзину по id пользователя
        /// </summary>
        // GET: /api/cart/{userId}

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetCartByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest();
            try
            {
                var cartList = await _cartService.FindAllAsync(userId, default);
                if(cartList is null)
                    return NotFound();

                var produtIds = cartList.Select(cart => cart.TeaId);
                var filter = Builders<Tea>.Filter.In(tea => tea.Id, produtIds);

                var teas = await _teaService.FindAllAsync(filter, default);

                return Ok( new { teas = teas });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///  Добавляет чай в корзину пользователя
        /// </summary>
        // POST: /api/cart/{userId}

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<IActionResult> AddTeaToCartAsync(CartAdd cartAdd)
        {
            if (cartAdd is null)
                return BadRequest();
            try
            {
                Cart cart = new Cart
                {
                    TeaId = cartAdd.TeaId,
                    UserId = cartAdd.UserId
                };

                bool isSuccess = await _cartService.SaveOrUpdateAsync(cart, default);
                if (isSuccess)
                    return Ok(new { cart = cart });

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Удаляет чай из корзины пользователя
        /// </summary>
        // DELETE: /api/cart/{userId}

        [Authorize(Roles = "User,Admin")]
        [HttpDelete]
        [Route("{cartId}")]
        public async Task<IActionResult> DeleteProductFromCartAsync(string cartId)
        {
            if (string.IsNullOrEmpty(cartId)) 
                return BadRequest();
            try
            {
                long deletedCount = await _cartService.DeleteAsync(cartId, default);
                if(deletedCount > 0)
                    return Ok(new {deletedCount = deletedCount});

                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
