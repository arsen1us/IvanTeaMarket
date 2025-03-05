using MongoDB.Driver;
using CustomerChurmPrediction.Entities.ChurnPredictionEntity;
using static CustomerChurmPrediction.Utils.CollectionName;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;

namespace CustomerChurmPrediction.Services
{
    public interface IChurnPredictionService : IBaseService<ChurnPrediction> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<ChurnPredictionModel>> GetChurnPredictionModelsAsync(CancellationToken? cancellationToken = default);
    }

    /// <summary>
    /// Сервис для работы с предсказаниями оттока пользователей
    /// </summary>
    /// <param name="client"></param>
    /// <param name="config"></param>
    /// <param name="logger"></param>
    /// <param name="_productService"></param>
    /// <param name="_environment"></param>
    public class ChurnPredictionService(
        IMongoClient _client,
        IConfiguration _config,
        ILogger<CartService> _logger,
        IProductService _productService,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService,
        IWebHostEnvironment _environment)
        : BaseService<ChurnPrediction>(_client, _config, _logger, _environment, _hubContext, _userConnectionService, ChurnPredictions), IChurnPredictionService
    {
        public async Task<List<ChurnPredictionModel>> GetChurnPredictionModelsAsync(CancellationToken? cancellationToken = default)
        {
            try
            {
                var pipeline = new[]
                {
                    // 
                    new BsonDocument("$addFields",
                        new BsonDocument
                        {
                            { "userIdString", new BsonDocument("$toString", "$_id") } // Преобразуем _id в строку
                        }),
                    new BsonDocument("$lookup",
                        new BsonDocument
                        {
                            { "from", "churnPredictions" },
                            { "localField", "userIdString" },
                            { "foreignField", "userId" },
                            { "as", "churnPredictions" }
                        }),
                    new BsonDocument("$unwind",
                        new BsonDocument
                        {
                            { "path", "$churnPredictions" },
                            { "preserveNullAndEmptyArrays", true }
                        }),
                    new BsonDocument("$project",
                        new BsonDocument
                        {
                            { "_id", 1 }, // Включаем поле _id
                            { "User", "$$ROOT" },
                            { "ChurnPrediction", "$churnPredictions" }
                        })
                };

                var result = (await UserCollection.AggregateAsync<ChurnPredictionModel>(pipeline)).ToList();

                return result;
            }
            catch(Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
