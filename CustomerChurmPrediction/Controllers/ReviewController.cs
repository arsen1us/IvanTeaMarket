using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Services;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/review")]
    public class ReviewController : Controller
    {
        IReviewService _reviewService;
        IUserService _userService;
        ILogger<ReviewController> _logger;
        public ReviewController(IReviewService reviewService, IUserService userService, ILogger<ReviewController> logger)
        {
            _reviewService = reviewService;
            _userService = userService;
            _logger = logger;

        }
    }
}
