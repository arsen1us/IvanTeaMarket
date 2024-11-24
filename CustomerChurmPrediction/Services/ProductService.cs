using CustomerChurmPrediction.Entities.ProductEntity;
using MongoDB.Driver;
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

    }
    public class ProductService(IMongoClient client, IConfiguration config, ILogger<ProductService> logger) 
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
    }
}
