using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Services;
using CustomerChurmPrediction.Entities.ReviewEntity;
using Microsoft.AspNetCore.Authorization;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/review")]
    public class ReviewController(
        IReviewService _reviewService,
        IUserService _userService,
        ILogger<ReviewController> _logger) : Controller
    {
        /// <summary>
        /// Получает список ReviewModels по id чая
        /// </summary>
        // GET: /api/review/{teaId}

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [Route("{teaId}")]
        public async Task<IActionResult> GetReviewModelsByProductIdAsync(string teaId)
        {
            if (string.IsNullOrEmpty(teaId))
            {

                return BadRequest();
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;

            try
            {
                var reviewModelList = await _reviewService.GetReviewModelsByProductIdAsync(teaId, cancellationToken);

                if (reviewModelList is null)
                {

                    return BadRequest();
                }

                return Ok(new { reviewModelList = reviewModelList });
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Создаёт отзыв
        /// </summary>
        /// <param name="reviewAdd"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        // POST: /api/review

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<IActionResult> AddReviewAsync(ReviewAdd reviewAdd)
        {
            if (reviewAdd is null)
            {

                return BadRequest();
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;

            try
            {
                Review review = new Review
                {
                    UserId = reviewAdd.UserId,
                    TeaId = reviewAdd.TeaId,
                    Text = reviewAdd.Text,
                    Grade = reviewAdd.Grade,
                    CreateTime = DateTime.Now,
                    LastTimeUserUpdate = DateTime.Now,
                };

                bool isSuccess = await _reviewService.SaveOrUpdateAsync(review, cancellationToken);

                if (isSuccess)
                {

                    return Ok(new { review = review });
                }

                return StatusCode(500);
            }
            catch (Exception ex)
            {

                throw new Exception();
            }
        }

        /// <summary>
        /// Удаляет отзыв
        /// </summary>
        /// <param name="reviewAdd"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        // DELETE: /api/review/{id}

        [Authorize(Roles = "User,Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteReviewAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {

                return BadRequest();
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;

            try
            {
                long deletedCount = await _reviewService.DeleteAsync(id, cancellationToken);

                if(deletedCount == 0)
                {

                    return StatusCode(500);
                }

                return Ok(new { deletedCount = deletedCount});
            }
            catch (Exception ex)
            {

                throw new Exception();
            }
        }
    }
}
