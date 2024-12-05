using CustomerChurmPrediction.Entities.ProductEntity;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface IProductService : IBaseService<Product>
    {
        /// <summary>
        ///  Получить список продуктов по id категории
        /// </summary>
        public Task<List<Product>> FindByCategoryIdAsync(string categoryId, CancellationToken? cancellationToken = default);

        /// <summary>
        ///  Получить список продуктов по id компании
        /// </summary>
        public Task<List<Product>> FindByCompanyIdAsync(string companyId, CancellationToken? cancellationToken = default);

        /// <summary>
        ///  Получить список продуктов по строке ввода
        /// </summary>
        public Task<List<Product>> FindBySearchStringAsync(string input, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Сохранить ссылки на изображения
        /// </summary>
        public Task<List<string>> UploadImagesAsync(IFormFileCollection images, CancellationToken? cancellationToken = default);
    }
    public class ProductService(IMongoClient client, IConfiguration config, ILogger<ProductService> logger, IWebHostEnvironment _environment) 
        : BaseService<Product>(client, config, logger, Products), IProductService
    {

        public async Task<List<Product>> FindByCategoryIdAsync(string categoryId, CancellationToken? cancellationToken = default)
        {
            if(string.IsNullOrEmpty(categoryId))
                throw new ArgumentNullException(nameof(categoryId));
            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.CategoryId, categoryId);
                var products = await base.FindAllAsync(filter, cancellationToken);

                return products;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Product>> FindByCompanyIdAsync(string companyId, CancellationToken? cancellationToken = default)
        {
            if (string.IsNullOrEmpty(companyId))
                throw new ArgumentNullException(nameof(companyId));
            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.CompanyId, companyId);
                var products = await base.FindAllAsync(filter, cancellationToken);

                return products;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<Product>> FindBySearchStringAsync(string input, CancellationToken? cancellationToken = default)
        {
            if(string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));
            try
            {
                // Регилярки для поиска по имени и описанию
                var filter = Builders<Product>.Filter.Or(
                    Builders<Product>.Filter.Regex(p => p.Name, new BsonRegularExpression(input, "i")),
                    Builders<Product>.Filter.Regex(p => p.Description, new BsonRegularExpression(input, "i"))
);
                var products = await base.FindAllAsync(filter);

                return products;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<string>> UploadImagesAsync(IFormFileCollection images, CancellationToken? cancellationToken = default)
        {
            if (images is null || images.Count == 0)
                // возвращаю пустой список
                return new List<string>();

            var uploadFilePaths = new List<string>();

            try
            {
                // Путь к папке uploads
                var updoadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                foreach (var image in images)
                {

                    var fileExtension = Path.GetExtension(image.FileName).ToLower();

                    // Если у файла другое расширение 
                    if (!allowedExtensions.Contains(fileExtension))
                        // возвращаю пустой список
                        return new List<string>();

                    // Id изображения
                    var imageId = Guid.NewGuid();
                    // Для продукта
                    var filePath = Path.Combine(imageId + fileExtension);
                    // Для сохранения в файл
                    var folderToSave = Path.Combine(updoadsFolder, imageId + fileExtension);

                    using (var fileStream = new FileStream(folderToSave, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    // Добавление ссылки на фото
                    uploadFilePaths.Add(filePath);
                }
                // Если успешно
                if(uploadFilePaths is not null)
                    return uploadFilePaths;
                // возвращаю пустой список
                return new List<string>();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
