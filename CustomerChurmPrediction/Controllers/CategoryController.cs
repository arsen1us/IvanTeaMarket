using CustomerChurmPrediction.Entities.CategoryEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController(ICategoryService _categoryService) : Controller
    {
        /// <summary>
        /// Получает список всех категорий
        /// </summary>
        // GET: /api/category

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var filter = Builders<Category>.Filter.Empty;
                var categories = await _categoryService.FindAllAsync(filter, default);

                if (categories is null)
                    return NotFound();

                return Ok(new { categories = categories });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Получает категорию по id
        /// </summary>
        // GET: /api/category/{categoryId}

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
        /// <summary>
        /// Создаёт категорию
        /// </summary>
        // POST: /api/category

        [Authorize(Roles = "Admin")]
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
        /// <summary>
        /// Изменяет категорию
        /// </summary>
        // PUT: /api/category/{categoryId}

        [Authorize(Roles = "Admin")]
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
        /// <summary>
        /// Удаляет категорию
        /// </summary>
        // DELETE: /api/category/{categoryId}

        [Authorize(Roles = "Admin")]
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
