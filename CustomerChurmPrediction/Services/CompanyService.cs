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
        public Task<Company> GetCompanyByProductId(string productId, CancellationToken? cancellationToken = default);
    }
    public class CompanyService(IMongoClient client, IConfiguration config, ILogger<CompanyService> logger, IProductService productService) 
        : BaseService<Company>(client, config, logger, Companies), ICompanyService 
    {
        public async Task<Company> GetCompanyByProductId(string productId, CancellationToken? cancellationToken = default)
        {
            if(string.IsNullOrEmpty(productId))
                throw new ArgumentNullException(nameof(productId));
            try
            {
                var product = await productService.FindByIdAsync(productId, cancellationToken);

                if(product is not null)
                {
                    string companyId = product.CompanyId;
                    
                    var company = await base.FindByIdAsync(companyId, cancellationToken);
                    if (company is null)
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
