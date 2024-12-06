using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Services;
using CustomerChurmPrediction.Entities.ReviewEntity;


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
        /// <summary>
        /// Получить писок ReviewModels по id продукта 
        /// </summary>
        // GET: /api/review/{productId}

        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GetReviewModelsByProductIdAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                return BadRequest();
            try
            {
                var reviewModelList = await _reviewService.GetReviewModelsByProductIdAsync(productId, default);

                if (reviewModelList is null)
                    return BadRequest();

                return Ok(new { reviewModelList = reviewModelList });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Добавить отзыв к продукту продукта 
        /// </summary>
        // POST: /api/review

        [HttpPost]
        public async Task<IActionResult> AddReviewAsync(ReviewAdd reviewAdd)
        {
            if (reviewAdd is null)
                return BadRequest();
            try
            {
                Review review = new Review
                {
                    UserId = reviewAdd.UserId,
                    ProductId = reviewAdd.ProductId,
                    Text = reviewAdd.Text,
                    Grade = reviewAdd.Grade,
                    CreateTime = DateTime.Now,
                    LastTimeUserUpdate = DateTime.Now,
                };

                bool isSuccess = await _reviewService.SaveOrUpdateAsync(review, default);

                if (isSuccess)
                    return Ok(new { review = review });

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}
