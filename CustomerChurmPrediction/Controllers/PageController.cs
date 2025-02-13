using CustomerChurmPrediction.Entities.PageEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/page")]
    public class PageController(IPageService _pageService) : Controller
    {
        // Получить число просмотров
        //[HttpGet]
        //public async Task<IActionResult> GetPageViewsByIdAsync(string pageId, CancellationToken? cancellationToken = default)
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
        //[HttpPost]
        //public async Task<IActionResult> AddViewAsync(PageAdd pageAdd, CancellationToken? cancellationToken = default)
        //{
        //    try
        //    {
        //        if (pageAdd is null || string.IsNullOrEmpty(pageAdd.UserId) || string.IsNullOrEmpty(pageAdd.PageUrl))
        //        {
        //            return BadRequest();
        //        }

        //        Page page = new Page
        //        {
        //            UserId = pageAdd.UserId,
        //            PageUrl = pageAdd.PageUrl,
        //        };

        //        bool result = await _pageService.SaveOrUpdateAsync(page, cancellationToken);

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
