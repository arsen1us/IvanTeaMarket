using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Services;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/coupon")]
    public class CouponController : Controller
    {
        ICouponService _couponService;
        IUserService _userService;
        ILogger<CouponController> _logger;

        public CouponController(ICouponService couponService, IUserService userService, ILogger<CouponController> logger)
        {
            _couponService = couponService;
            _userService = userService;
            _logger = logger;
        }
    }
}
