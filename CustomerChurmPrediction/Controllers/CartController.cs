using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Entities.CartEntity;


using Microsoft.AspNetCore.Authorization;
using NLog.LayoutRenderers;
using CustomerChurmPrediction.Entities.UserEntity;
using MongoDB.Bson;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/cart")]
    public class CartController : ControllerBase
    {
        ICartService _cartService;
        IUserService _userService;
        ILogger<CartController> _logger;
        public CartController(ICartService cartService, IUserService userService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _userService = userService;
            _logger = logger;
        }
        /// <summary>
        /// Получить корзину по id пользователя
        /// </summary>
        // GET: /api/cart/{userId}

        [Authorize(Roles = "User, Admin, Owner")]
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

                var productList = await _cartService.FindProductsFromCardByUserId(userId, default);

                return Ok( new { productList = productList });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Получить корзину по id пользователя
        /// </summary>
        // POST: /api/cart/{userId}

        [Authorize(Roles = "User, Admin, Owner")]
        [HttpPost]
        public async Task<IActionResult> AddProductToCartAsync(CartAdd cartAdd)
        {
            if (cartAdd is null)
                return BadRequest();
            try
            {
                Cart cart = new Cart
                {
                    ProductId = cartAdd.ProductId,
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
        /// Получить корзину по id пользователя
        /// </summary>
        // DELETE: /api/cart/{userId}

        [Authorize(Roles = "User, Admin, Owner")]
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
