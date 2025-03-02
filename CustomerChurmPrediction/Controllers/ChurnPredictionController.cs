using CustomerChurmPrediction.Entities;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("api/churn-prediction")]
    public class ChurnPredictionController(
        ILogger<ChurnPredictionController> _logger,
        IChurnPredictionService _predictionService) : Controller
    {
        /// <summary>
        /// Получить все предсказания
        /// </summary>
        /// <returns></returns>
        // GET: https://localhost:7299/api/churn-prediction
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<ChurnPrediction> predictionList = await _predictionService.FindAllAsync(default);
                return Ok(new { predictionList = predictionList });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
