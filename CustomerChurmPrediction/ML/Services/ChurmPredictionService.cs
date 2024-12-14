using CustomerChurmPrediction.ML.Entities;
using CustomerChurmPrediction.Services;
using static CustomerChurmPrediction.Utils.CollectionName;
using MongoDB.Driver;

namespace CustomerChurmPrediction.ML.Services
{
    public interface IChurmPredictionService : IBaseService<ChurnPrediction>
    {

    }
    public class ChurmPredictionService(IMongoClient client, IConfiguration config, ILogger<UserService> logger, IWebHostEnvironment _environment) 
        : BaseService<ChurnPrediction>(client, config, logger, _environment, ChurnPredictions)
    {
    }
}
