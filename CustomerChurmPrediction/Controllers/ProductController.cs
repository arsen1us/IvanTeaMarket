using CustomerChurmPrediction.Entities.ProductEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/product")]
    public class ProductController(
        IUserService _userService,
        IProductService _productService,
        ICompanyService _companyService,
        ILogger<ProductController> _logger,
        IHubContext<NotificationHub> _hubContext) : ControllerBase
    {
        /// <summary>
        /// Получить список всех продуктов
        /// </summary>
        // GET: https://localhost:7299/api/product

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var filter = Builders<Product>.Filter.Empty;
                List<Product> productList = await _productService.FindAllAsync(filter, default);

                _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(GetAllAsync)}] - Успешно получен список продуктов. Число записей: {productList.Count}");
                return Ok(new { productList = productList });
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetAllAsync)}] - Не удалось получить список продуктов. Детали ошибки: {ex.Message}");
                throw new Exception($"[{DateTime.UtcNow} Method: {nameof(GetAllAsync)}] - Не удалось получить список продуктов. Детали ошибки: {ex.Message}");
            }
        }

        /// <summary>
        /// Получить продукт по id
        /// </summary>
        // GET: https://localhost:7299/api/product/{productId}

        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GetByIdAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByIdAsync)}] - Параметр {nameof(productId)} не был передан");
                return BadRequest();
            }

            try
            {
                Product product = await _productService.FindByIdAsync(productId);
                if (product is null)
                {
                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByIdAsync)}] - Не удалось найти запись с id {productId}");
                    return NotFound();

                }

                _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(GetByIdAsync)}] - Запись с id {productId} успешно получена");
                return Ok(new { product = product });

            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByIdAsync)}] - Не удалосб получить запись с id {productId}. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Получить список продуктов по id категории
        /// </summary>
        // GET: https://localhost:7299/api/product/category/{categoryId}

        [HttpGet]
        [Route("category/{categoryId}")]
        public async Task<IActionResult> GetByCategoryIdAsync(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByCategoryIdAsync)}] - Параметр {nameof(categoryId)} не был передан");
                return BadRequest();
            }

            try
            {
                List<Product> productList = await _productService.FindByCategoryIdAsync(categoryId);
                if (productList is null)
                {
                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByCategoryIdAsync)}] - Не удалось найти запись с id категории {categoryId}");
                    return NotFound();
                }

                _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(GetByCategoryIdAsync)}] - Запись с id категории {categoryId} успешно получены. Число записей: {productList.Count}");
                return Ok(new { productList = productList });
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByCategoryIdAsync)}] - Не удалось найти получить записи с id категории {categoryId}. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Получить записи по номеру страницы и количеству элементов на странице
        /// </summary>
        // GET: https://localhost:7299/api/product/{pageSize, pageNumber}

        [HttpGet]
        [Route("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> FetchProductsAsync(int pageNumber, int pageSize)
        {
            try
            {
                List<Product> productList = await _productService.FindAllAsync(default, default);
                if (productList is not null)
                {
                    var resultList = productList
                        .Skip(pageNumber * pageSize)
                        .Take(pageSize)
                        .ToList();

                    _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(FetchProductsAsync)}] - Успешно получен список продуктов из {pageSize} записей для страницы {pageNumber}");
                    return Ok(new { productList = resultList });
                }

                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(FetchProductsAsync)}] - Не удалось найти список продуктов");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(FetchProductsAsync)}] - Произошла ошибка при получении списка продуктов из {pageSize} записей для страницы {pageNumber}. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Получить список продуктов по строке поиска
        /// </summary>
        // GET: https://localhost:7299/api/product/search/{input}

        [HttpGet]
        [Route("search/{input}")]
        public async Task<IActionResult> GetBySearchStringAsync(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetBySearchStringAsync)}] - Параметр {nameof(input)} не был передан");
                return BadRequest();
            }

            try
            {
                var productList = await _productService.FindBySearchStringAsync(input, default);

                if (productList is not null)
                {
                    _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(GetBySearchStringAsync)}] - Успешно получен список продуктов со строкой запроса {input}. Число запаисей: {productList.Count}");
                    return Ok(new { productList = productList });
                }

                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetBySearchStringAsync)}] - Не удалось найти список записей со строкой запроса {input}");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetBySearchStringAsync)}] - Произошла ошибка во время получения списка записей со строкой запроса {input}. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Получить список продуктов по id компании
        /// </summary>
        // GET: https://localhost:7299/api/product/company/{companyId}

        [Authorize(Roles = "Admin, Owner")]
        [HttpGet]
        [Route("company/{companyId}")]
        public async Task<IActionResult> GetByCompanyIdAsync(string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByCompanyIdAsync)}] - Параметр {nameof(companyId)} не был передан");
                return BadRequest();
            }

            try
            {
                var company = await _companyService.FindByIdAsync(companyId, default);
                if (company is null)
                {
                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByCompanyIdAsync)}] - Не удалось найти компанию с id {companyId}");
                    return NotFound();
                }

                var productList = await _productService.FindByCompanyIdAsync(companyId, default);
                if (productList is not null)
                {
                    _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(GetByCompanyIdAsync)}] - Успешно получен список продуктов с id компании {companyId}");
                    return Ok(new { productList = productList });
                }

                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByCompanyIdAsync)}] - Не удалось найти записи для компании с id {companyId}");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByCompanyIdAsync)}] - Не удалось получить найти записи для компании с id {companyId}. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Создать продукт
        /// </summary>
        // POST: https://localhost:7299/api/product/{productId}

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromForm] ProductDto productDto)
        {
            if (productDto is null)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddAsync)}] - Параметр {nameof(productDto)} не был передан");
                return BadRequest();
            }

            // Получение и запись изображений
            if (productDto.Images is null || productDto.Images.Count == 0)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddAsync)}] - Обязательно необходимо передать список изображений");
                return BadRequest();
            }

            try
            {
                Product product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    CategoryId = productDto.CategoryId,
                    CompanyId = productDto.CompanyId,
                    Count = productDto.Quantity,
                    Price = productDto.Price,
                    CreatorId = productDto.UserId,
                    UserIdLastUpdate = productDto.UserId
                };

                // Загрузка изображений на сервер
                List<string> imageSrcs = await _productService.UploadImagesAsync(productDto.Images, default);
                if (imageSrcs is null)
                {
                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddAsync)}] - Не удалось сохранить изображения для продукта");
                    return StatusCode(500);
                }

                product.ImageSrcs = imageSrcs;

                bool isSuccess = await _productService.SaveOrUpdateAsync(product, default);
                if (isSuccess)
                {
                    _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(AddAsync)}] - Продукт успешно добавлен");
                    await _hubContext.Clients.All.SendAsync("ReceiveNotification", "Продукт успешно добавлен!");
                    return Ok(new { product = product });
                }

                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddAsync)}] - Не удалось сохранить изображения для продукта");
                return StatusCode(500);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Изменить продукт
        /// </summary>
        // PUT: https://localhost:7299/api/product/{productId}

        [HttpPut]
        [Route("{productId}")]
        public async Task<IActionResult> UpdateAsync(string productId, [FromBody] ProductUpdate productUpdate)
        {
            if (productId is null || productUpdate is null)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(UpdateAsync)}] - Параметры {nameof(productId)}, {nameof(productUpdate)} не был передан");
                return BadRequest();
            }

            try
            {
                var product = await _productService.FindByIdAsync(productId, default);
                if (product != null)
                {
                    product.Name = productUpdate.Name;
                    var isSuccess = await _productService.SaveOrUpdateAsync(product, default);
                    if (isSuccess)
                    {
                        _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(UpdateAsync)}] - Продукт с id {productId} успешно обновлён");
                        return Ok(new { product = product });
                    }

                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(UpdateAsync)}] - Не удалось обновить продукт с id {nameof(productId)}");
                    return StatusCode(500);
                }
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(UpdateAsync)}] - Не удалось найти продукт с id {nameof(productId)}");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(UpdateAsync)}] - Произошла ошибка при обновлении продукта с id {nameof(productId)}. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Удалить продукт
        /// </summary>
        // Delete: https://localhost:7299/api/product/{productId}

        [HttpDelete]
        [Route("{productId}")]
        public async Task<IActionResult> DeleteAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(DeleteAsync)}] - Параметр {nameof(productId)} не был передан");
                return BadRequest();
            }

            try
            {
                var deletedCount = await _productService.DeleteAsync(productId);
                if (deletedCount > 0)
                {
                    _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(DeleteAsync)}] - Продукт с id {productId} успешно удалён. Число удалённых записей: {deletedCount}");
                    return Ok(new { deletedCount = deletedCount });
                }

                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(DeleteAsync)}] - Не удалось найти продукт с id {nameof(productId)}");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(UpdateAsync)}] - Произошла ошибка при удалении продукта с id {nameof(productId)}. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
