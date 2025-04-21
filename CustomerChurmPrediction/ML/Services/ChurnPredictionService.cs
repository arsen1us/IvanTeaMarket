using CustomerChurmPrediction.Entities.UserEntity;
using CustomerChurmPrediction.ML.Entities.ChurnPredictionEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.ML.Services
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
    /// <param name="_environment"></param>
    public class ChurnPredictionService(
        IMongoClient _client,
        IConfiguration _config,
        ILogger<CartService> _logger,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService,
        IWebHostEnvironment _environment)
        : BaseService<ChurnPrediction>(_client, _config, _logger, _environment, _hubContext, _userConnectionService, ChurnPredictions), IChurnPredictionService
    {
        public async Task<List<ChurnPredictionModel>> GetChurnPredictionModelsAsync(CancellationToken? cancellationToken = default)
        {
            try
            {
                var join = from churnPrediction in Collection.AsQueryable()
                           join user in UserCollection.AsQueryable() on churnPrediction.UserId equals user.Id
                           select new ChurnPredictionModel
                           {
                               User = user,
                               ChurnPrediction = churnPrediction
                           };

                List<ChurnPredictionModel> result = join.ToList();

                // var result = (await UserCollection.AggregateAsync<ChurnPredictionModel>(pipeline)).ToList();

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
