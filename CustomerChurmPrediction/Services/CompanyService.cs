using CustomerChurmPrediction.Entities.CompanyEntity;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface ICompanyService : IBaseService<Company> 
    {
        /// <summary>
        /// Получить компанию по id продукта
        /// </summary>
        public Task<Company> GetByProductIdAsync(string productId, CancellationToken? cancellationToken = default);
    }
    public class CompanyService(IMongoClient client, IConfiguration config, ILogger<CompanyService> logger, IProductService productService, IWebHostEnvironment _environment) 
        : BaseService<Company>(client, config, logger, _environment, Companies), ICompanyService 
    {
        public async Task<Company> GetByProductIdAsync(string productId, CancellationToken? cancellationToken = default)
        {
            if(string.IsNullOrEmpty(productId))
                throw new ArgumentNullException(nameof(productId));
            try
            {
                // получить продукт по id
                var product = await productService.FindByIdAsync(productId, cancellationToken);

                if(product is not null)
                {
                    // получить id компании
                    string companyId = product.CompanyId;
                    
                    var company = await FindByIdAsync(companyId, cancellationToken);

                    return company;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
