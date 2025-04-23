using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Services;
using CustomerChurmPrediction.Entities.PromotionEntity;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/promotion")]
    public class PromotionController(
        IPromotionService _promotionService,
        ILogger<PromotionController> _logger) : Controller
    {
        /// <summary>
        /// Получает список всех рекламных постов
        /// </summary>
        // GET: api/promotion

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPromotionListAsync()
        {
            try
            {
                var filter = Builders<Promotion>.Filter.Empty;
                var promotionList = await _promotionService.FindAllAsync(filter, default);

                return Ok(new { promotionList = promotionList });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Добавляет рекламу
        /// </summary>
        // POST: /api/promotion

        [Authorize(Roles = "Admin, Owner")]
        [HttpPost]
        public async Task<IActionResult> AddPromotionAsync(PromotionAdd promotionAdd)
        {
            if (promotionAdd is null) 
            {
                return BadRequest();
            }
            try
            {
                Promotion promotion = new Promotion
                {
                    Title = promotionAdd.Title,
                    Content = promotionAdd.Content,
                    ImageUrl = promotionAdd.ImageUrl,
                    LinkUrl = promotionAdd.LinkUrl,
                    StartDate = promotionAdd.StartDate,
                    EndDate = promotionAdd.EndDate,
                    CompanyId = promotionAdd.CompanyId
                };

                bool isSuccess = await _promotionService.SaveOrUpdateAsync(promotion);
                if(isSuccess)
                {
                    return Ok(new { promotion = promotion });
                }

                // Возвращаю ошибку сервера
                return StatusCode(500);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Изменяет рекламу
        /// </summary>
        // PUT: /api/promotion/{promotionId}

        [Authorize(Roles = "Admin, Owner")]
        [HttpPut("{promotionId}")]
        public async Task<IActionResult> UpdatePromotionAsync(string promotionId, PromotionUpdate promotionUpdate)
        {
            if (string.IsNullOrEmpty(promotionId) || promotionUpdate is null)
            {
                return BadRequest();
            }
            try
            {
                var promotion = await _promotionService.FindByIdAsync(promotionId, default);

                if (promotion != null)
                {
                    promotion.Title = promotionUpdate.Title;
                    promotion.Content = promotionUpdate.Content;
                    promotion.ImageUrl = promotionUpdate.ImageUrl;
                    promotion.LinkUrl = promotionUpdate.LinkUrl;
                    promotion.StartDate = promotionUpdate.StartDate;
                    promotion.EndDate = promotionUpdate.EndDate;

                    bool isSuccess = await _promotionService.SaveOrUpdateAsync(promotion);
                    if (isSuccess)
                    {
                        return Ok(new { promotion = promotion });
                    }

                    // Возвращаю ошибку сервера
                    return StatusCode(500);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Удаляет рекламу
        /// </summary>
        // DELETE: /api/promotion/{promotionId}

        [Authorize(Roles = "Admin, Owner")]
        [HttpDelete]
        [Route("{promotionId}")]
        public async Task<IActionResult> DeletePromotionAsync(string promotionId)
        {
            if (string.IsNullOrEmpty(promotionId))
                return BadRequest();
            try
            {
                long deletedCount = await _promotionService.DeleteAsync(promotionId, default);
                if (deletedCount > 0)
                {
                    return Ok(new { deletedCount = deletedCount });
                }

                // Если не удалено ни одной записи, возвращаю NotFound
                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
