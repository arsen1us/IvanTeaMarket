using CustomerChurmPrediction.Entities.PromotionEntity;
using Microsoft.AspNetCore.SignalR;
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
    public class PromotionService(
        IMongoClient client,
        IConfiguration config,
        ILogger<PromotionService> logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService) 
        : BaseService<Promotion>(client, config, logger, _environment, _hubContext, _userConnectionService, Promotions), IPromotionService
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
