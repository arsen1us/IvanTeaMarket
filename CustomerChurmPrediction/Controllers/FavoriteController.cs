using CustomerChurmPrediction.Entities.FavoriteEntity;
using CustomerChurmPrediction.Entities.ProductEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/favorite")]
    public class FavoriteController : ControllerBase
    {
        IFavoriteService _favoriteService;
        IProductService _productService;
        IUserService _userService;
        ILogger<FavoriteController> _logger;

        public FavoriteController(IFavoriteService favoriteService, IProductService productService, IUserService userService, ILogger<FavoriteController> logger)
        {
            _favoriteService = favoriteService;
            _productService = productService;
            _userService = userService;
            _logger = logger;
        }
        // Получить список продуктов из избранного
        // GET: /api/favorite

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetFavoriteListAsync(string userId)
        {
            if (userId == null) 
                return BadRequest();
            try
            {
                var favoriteList = await _favoriteService.FindAllAsync(default, userId);

                // Если в списке избранного есть сущности
                if (favoriteList != null && favoriteList.Count > 0)
                {
                    var productIds = favoriteList.Select(f => f.ProductId);
                    var filter = Builders<Product>.Filter.In(p => p.Id, productIds);

                    var products = await _productService.FindAllAsync(filter, default);

                    // Если всё ок
                    if (products != null && products.Count > 0)
                        return Ok(new { products = products });

                    // Если не ок
                    return NotFound();
                }
                // Если список пуст
                else if (favoriteList != null)
                    return Ok();
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Добавить запись в избранное
        // POST: /api/favorite

        [HttpPost]
        public async Task<IActionResult> AddToFavoriteAsync([FromBody] FavoriteAdd favoriteAdd)
        {
            if (favoriteAdd is null
                || string.IsNullOrEmpty(favoriteAdd.UserId)
                || string.IsNullOrEmpty(favoriteAdd.ProductId))
                return BadRequest();

            try
            {
                // Проверка, существует ли продукт и пользователь
                var product = await _productService.FindByIdAsync(favoriteAdd.ProductId, default);
                var user = await _userService.FindByIdAsync(favoriteAdd.UserId, default);
                if (product != null && user != null)
                {
                    Favorite favorite = new Favorite
                    {
                        UserId = favoriteAdd.UserId,
                        ProductId = favoriteAdd.ProductId
                    };
                    // Сохраняю запись в избранном
                    var isSuccess = await _favoriteService.SaveOrUpdateAsync(favorite);
                    if (isSuccess)
                        return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Удалить запись из избранного
        // POST: /api/favorite/{favoriteId}

        [HttpDelete]
        [Route("{favoriteId}")]
        public async Task<IActionResult> DeleteFromFavoriteAsync(string favoriteId)
        {
            if (string.IsNullOrEmpty(favoriteId))
                return BadRequest();

            try
            {
                var favorite = await _favoriteService.FindByIdAsync(favoriteId, default);
                if (favorite != null)
                {
                    var deletedCount = await _favoriteService.DeleteAsync(favoriteId, default);
                    if (deletedCount > 0)
                        return Ok(new { deletedCount = deletedCount });

                    return NotFound();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
