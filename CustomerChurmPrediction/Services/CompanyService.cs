using CustomerChurmPrediction.Entities.CompanyEntity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface ICompanyService : IBaseService<Company> 
    {
        /// <summary>
        /// Получить компанию по id продукта
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Company> GetByProductIdAsync(string productId, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Получить компанию по id пользователя 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Company> FindByUserIdAsync(string userId, CancellationToken? cancellationToken = default);
    }
    public class CompanyService(
        IMongoClient client,
        IConfiguration config,
        ILogger<CompanyService> logger, 
        IProductService productService, 
        IWebHostEnvironment _environment, 
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService) 
        : BaseService<Company>(client, config, logger, _environment, _hubContext, _userConnectionService, Companies), ICompanyService 
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

        public async Task<Company> FindByUserIdAsync(string userId, CancellationToken? cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            try
            {
                var filter = Builders<Company>.Filter.Eq(company => company.OwnerId, userId);

                var company = (await FindAllAsync(filter, cancellationToken)).FirstOrDefault();
                return company;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
