using CustomerChurmPrediction.Entities.ReviewEntity;
using CustomerChurmPrediction.Entities.UserEntity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface IReviewService : IBaseService<Review>
    {
        /// <summary>
        /// Получить список ReviewModel по id продукта
        /// </summary>
        public Task<List<ReviewModel>> GetReviewModelsByProductIdAsync(string productId, CancellationToken? cancellationToken = default);
    }
    public class ReviewService(
        IMongoClient client,
        IConfiguration config,
        ILogger<ReviewService> logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService) 
        : BaseService<Review>(client, config, logger, _environment, _hubContext, _userConnectionService, Reviews), IReviewService
    {
        public async Task<List<ReviewModel>> GetReviewModelsByProductIdAsync(string productId, CancellationToken? cancellationToken = default)
        {
            var userCollection = Database.GetCollection<User>(Users);

            if (string.IsNullOrWhiteSpace(productId))
                throw new ArgumentNullException(nameof(productId));
            try
            {
                var reviewModels = from review in Collection.AsQueryable()
                                   join user in userCollection.AsQueryable() on review.UserId equals user.Id
                                   where review.ProductId == productId
                                   select new ReviewModel
                                   {
                                       User = user,
                                       Review = review
                                   };

                return await reviewModels.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
