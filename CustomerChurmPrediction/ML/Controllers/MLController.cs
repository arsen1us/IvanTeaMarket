using CustomerChurmPrediction.ML.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChurmPrediction.ML.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/ml")]
    public class MLController : Controller
    {
        IChurmPredictionService _churmPredictionService;

        public MLController(IChurmPredictionService churmPredictionService)
        {
            _churmPredictionService = churmPredictionService;
        }

        ///<summary>
        /// Получить прогноз по id пользователя
        /// </summary>
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetPredictByIdAsync(string userId)
        {
            if(string.IsNullOrEmpty(userId))
                return BadRequest();
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
