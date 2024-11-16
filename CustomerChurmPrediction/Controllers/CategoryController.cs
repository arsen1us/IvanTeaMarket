using CustomerChurmPrediction.Entities.CategoryEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : Controller
    {
        ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var filter = Builders<Category>.Filter.Empty;
                var categoryList = await _categoryService.FindAllAsync(filter, default);

                if (categoryList != null && categoryList.Count > 0)
                    return Ok(new { categoryList = categoryList });

                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        [Route("{categoryId}")]
        public async Task<IActionResult> GetByIdAsync(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
                return BadRequest();

            try
            {
                var category = await _categoryService.FindByIdAsync(categoryId, default);
                if (category != null)
                    return Ok(new { category = category } );

                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CategoryAdd categoryAdd)
        {
            if (categoryAdd is null)
                return BadRequest();

            try
            {
                Category category = new Category
                {
                    Name = categoryAdd.Name
                };
                bool isSuccess = await _categoryService.SaveOrUpdateAsync(category, default);

                if (isSuccess)
                    return Ok(new { category = category });
                // Возвращаю ошибку сервера (Internal provider error)
                return StatusCode(500);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPut]
        [Route("{categoryId}")]
        public async Task<IActionResult> UpdateAsync(string categoryId, [FromBody] Category categoryUpdate)
        {
            if (string.IsNullOrEmpty(categoryId) || categoryUpdate is null)
                return BadRequest();
            try
            {
                Category category = await _categoryService.FindByIdAsync(categoryId, default);
                if (category is null)
                    // Если не удалось найти кагерорию с данным id
                    return NotFound();

                category.Name = categoryUpdate.Name;
                bool isSuccess = await _categoryService.SaveOrUpdateAsync(category, default);

                if (isSuccess)
                    return Ok(new { category });

                // Возвращаю ошибку сервера (Internal provider error)
                return StatusCode(500);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{categoryId}")]
        public async Task<IActionResult> DeleteByIdAsync(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
                return BadRequest();
            try
            {
                var deletedCount = await _categoryService.DeleteAsync(categoryId, default);
                if (deletedCount > 0)
                    return Ok(new { deletedCount = deletedCount });

                // Возврашаю NotFount, если количество удалённых запесей равно нулю
                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
