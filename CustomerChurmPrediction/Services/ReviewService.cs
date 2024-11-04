using CustomerChurmPrediction.Entities;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface IReviewService : IBaseService<Review>
    {

    }
    public class ReviewService(IMongoClient client, IConfiguration config, ILogger<ReviewService> logger) 
        : BaseService<Review>(client, config, logger, Reviews), IReviewService
    {

    }
}
