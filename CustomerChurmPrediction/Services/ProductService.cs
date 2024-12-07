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
    }
    public class ProductService(IMongoClient client, IConfiguration config, ILogger<ProductService> logger, IWebHostEnvironment _environment) 
        : BaseService<Product>(client, config, logger, _environment, Products), IProductService
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
    }
}
