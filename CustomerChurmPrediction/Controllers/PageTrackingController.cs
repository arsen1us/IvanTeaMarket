using CustomerChurmPrediction.Entities.PageEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/page")]
    public class PageTrackingController(IPageService _pageService) : Controller
    {
        // Получить число просмотров
        //[HttpGet]
        //public async Task<IActionResult> GetPageViewsByIdAsync(string pageId)
        //{
        //    try
        //    {
        //        if(string.IsNullOrEmpty(pageId))
        //        {
        //            return BadRequest();
        //        }
        //        long viewsCount = await _pageService.FindCountByIdAsync(pageId, cancellationToken);
        //        return Ok(viewsCount);
        //    }
        //    catch(Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //}

        // Добавить просмотр для текущего пользователя
        [HttpPost]
        public async Task<IActionResult> AddViewAsync(PageAdd pageAdd)
        {
            try
            {
                if (pageAdd is null || string.IsNullOrEmpty(pageAdd.UserId) || string.IsNullOrEmpty(pageAdd.PageUrl))
                {
                    return BadRequest();
                }

                Page page = new Page
                {
                    UserId = pageAdd.UserId,
                    PageUrl = pageAdd.PageUrl,
                    LastTimeUserUpdate = DateTime.UtcNow,
                    CreatorId = pageAdd.UserId,
                    UserIdLastUpdate = pageAdd.UserId
                };

                bool result = await _pageService.SaveOrUpdateAsync(page, default);

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
