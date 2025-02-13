﻿using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Services;
using MongoDB.Driver;
using CustomerChurmPrediction.Entities.ProductEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

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
        // GET: api/product

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var filter = Builders<Product>.Filter.Empty;
                List<Product> productList = await _productService.FindAllAsync(filter, default);

                return Ok( new {productList = productList } );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Получить продукт по id
        /// </summary>
        // GET: api/product/{productId}

        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GetByIdAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                return BadRequest();

            try
            {
                Product product = await _productService.FindByIdAsync(productId);
                if (product is null)
                    return NotFound();

                return Ok( new { product = product } );

            }
            catch(Exception ex)
            {
				throw new Exception(ex.Message);
			}
        }
        /// <summary>
        /// Получить список продуктов по id категории
        /// </summary>
        // GET: api/product/category/{categoryId}

        [HttpGet]
        [Route("category/{categoryId}")]
        public async Task<IActionResult> GetByCategoryIdAsync(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
                return BadRequest();
            try
            {
                List<Product> productList = await _productService.FindByCategoryIdAsync(categoryId);
                if (productList is null)
                    return NotFound();

                return Ok(new { productList = productList });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Получить определённое число продуктов
        /// </summary>
        // GET: api/product/{pageSize, pageNumber}

        [HttpGet]
        [Route("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> FetchProductsAsync(string pageNumber, string pageSize)
        {
            if (string.IsNullOrEmpty(pageNumber) && string.IsNullOrEmpty(pageSize))
                return BadRequest();
            try
            {
                if(Int32.TryParse(pageNumber, out int number) && Int32.TryParse(pageSize, out int size))
                {
                    List<Product> productList = await _productService.FindAllAsync(default, default);
                    if (productList is not null)
                    {
                        var resultList = productList
                            .Skip(number * size)
                            .Take(size)
                            .ToList();
                        return Ok(new {productList = resultList});
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Получить список продуктов по строке поиска
        /// </summary>
        // GET: /api/product/search/{input}

        [HttpGet]
        [Route("search/{input}")]
        public async Task<IActionResult> GetBySearchStringAsync(string input)
        {
            if(string.IsNullOrEmpty(input))
                return BadRequest();
            try
            {
                var productList = await _productService.FindBySearchStringAsync(input, default);

                if (productList is not null)
                    return Ok(new { productList = productList });
                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Получить список продуктов по id компании
        /// </summary>
        // GET: api/product/company/{companyId}

        [Authorize(Roles = "Admin, Owner")]
        [HttpGet]
        [Route("company/{companyId}")]
        public async Task<IActionResult> GetByCompanyIdAsync(string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
                return BadRequest();
            try
            {
                var company = await _companyService.FindByIdAsync(companyId, default);
                if (company is null)
                    return NotFound();

                var productList = await _productService.FindByCompanyIdAsync(companyId, default);

                if (productList is null)
                    return NotFound();

                return Ok(new { productList = productList });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Добавить продукт
        /// </summary>
        // POST: api/product/{productId}

        public class ProductDto
        {
            public string Name { get; set; } = null!;
            public string Description { get; set; } = null!;
            public string CategoryId { get; set; } = null!;
            public int Count { get; set; }
            public decimal Price { get; set; }
            public string CompanyId { get; set; } = null!;
            public IFormFileCollection? Images { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromForm] ProductDto productDto)
        {
            if(productDto is null)
                return BadRequest();
			try
			{
                Product product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    CategoryId = productDto.CategoryId,
                    CompanyId = productDto.CompanyId,
                    Count = productDto.Count,
                    Price = productDto.Price,
                };

                // Получение и запись изображений
                if (productDto.Images is null || productDto.Images.Count == 0)
                    return BadRequest();

                List<string> imageSrcs = await _productService.UploadImagesAsync(productDto.Images, default);

                if (imageSrcs is null)
                    return StatusCode(500);

                product.ImageSrcs = imageSrcs;

                bool isSuccess = await _productService.SaveOrUpdateAsync(product, default);
                if (isSuccess)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveNotification", "Продукт успешно добавлен!");
                    return Ok(new { product = product });
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
        /// Изменить продукт
        /// </summary>
        // PUT: api/product/{productId}

        [HttpPut]
        [Route("{productId}")]
        public async Task<IActionResult> UpdateAsync(string productId, [FromBody] ProductUpdate productUpdate)
        {
			if (productId is null || productUpdate is null)
				return BadRequest();
			try
			{
                var product = await _productService.FindByIdAsync(productId, default);
                if(product != null)
                {
                    product.Name = productUpdate.Name;
                    var isSuccess = await _productService.SaveOrUpdateAsync(product, default);
                    if(isSuccess)
                        return Ok(new {product = product} );

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
        /// Удалить продукт
        /// </summary>
        // Delete: api/product/{productId}

        [HttpDelete]
        [Route("{productId}")]
        public async Task<IActionResult> DeleteAsync(string productId)
        {
			if (string.IsNullOrEmpty(productId))
				return BadRequest();
			try
			{
                var deletedCount = await _productService.DeleteAsync(productId);
                if (deletedCount > 0)
                    return Ok(new { deletedCount = deletedCount });

                return NotFound();
			}
			catch (Exception ex)
			{
                throw new Exception(ex.Message);
            }
		}
    }
}
