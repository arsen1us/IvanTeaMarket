﻿using CustomerChurmPrediction.Entities.ChurnPredictionEntity;
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
    }
}
