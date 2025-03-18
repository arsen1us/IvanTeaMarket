using CustomerChurmPrediction.ML.Entities;
using CustomerChurmPrediction.ML.Entities.ChurnPredictionEntity;
using CustomerChurmPrediction.ML.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("api/churn-prediction")]
    public class ChurnPredictionController(
        ILogger<ChurnPredictionController> _logger,
        IChurnPredictionService _predictionService,
        IMLModelInputService _mLModelInputService) : Controller
    {
        /// <summary>
        /// Получить все предсказания
        /// </summary>
        /// <returns></returns>
        // GET: https://localhost:7299/api/churn-prediction

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<ChurnPredictionModel> churnPredictionList = await _predictionService.GetChurnPredictionModelsAsync();
                return Ok(new { churnPredictionList = churnPredictionList });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Получить объект MLModelInput по userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        [Route("ml-input/{userId}")]
        public async Task<IActionResult> GetMLModelInputByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("");
                return BadRequest($"Параметр {nameof(userId)} равен null или пуст");
            }
            try
            {
                MLModelInput MLModelInput = await _mLModelInputService.FindOneByUserIdAsync(userId, default);

                if(MLModelInput is not null)
                {
                    _logger.LogInformation("");
                    return Ok(new { MLModelInput = MLModelInput });
                }

                _logger.LogError("");
                return NotFound();
            }
            catch(Exception ex)
            {
                _logger.LogError("");
                throw new Exception(ex.Message);
            }
        }
    }
}
