using CustomerChurmPrediction.Entities.PromotionEntity;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface IPromotionService : IBaseService<Promotion>
    {
        /// <summary>
        /// Получить список рекламных постов по id компании
        /// </summary>
        public Task<List<Promotion>> GetByCompanyIdAsync(string companyId, CancellationToken? cancellationToken = default);
    }
    public class PromotionService(IMongoClient client, IConfiguration config, ILogger<PromotionService> logger, IWebHostEnvironment _environment) 
        : BaseService<Promotion>(client, config, logger, _environment, Promotions), IPromotionService
    {
        public async Task<List<Promotion>> GetByCompanyIdAsync(string companyId, CancellationToken? cancellationToken = default)
        {
            if (string.IsNullOrEmpty(companyId))
                throw new ArgumentNullException();
            try
            {
                var filter = Builders<Promotion>.Filter.Eq(p => p.CompanyId, companyId);
                var promotionList = await base.FindAllAsync(filter, cancellationToken);

                return promotionList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
