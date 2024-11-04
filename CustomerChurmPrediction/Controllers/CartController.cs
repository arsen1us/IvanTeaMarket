using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;

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
    }
}
