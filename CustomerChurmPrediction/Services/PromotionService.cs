using CustomerChurmPrediction.Entities;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface IPromotionService : IBaseService<Promotion>
    {

    }
    public class PromotionService(IMongoClient client, IConfiguration config, ILogger<PromotionService> logger) 
        : BaseService<Promotion>(client, config, logger, Promotions), IPromotionService
    {
    }
}
