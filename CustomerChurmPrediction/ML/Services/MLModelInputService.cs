using CustomerChurmPrediction.ML.Entities;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;


namespace CustomerChurmPrediction.ML.Services
{
    public interface IMLModelInputService : IBaseService<MLModelInput>
    {
    }


    public class MLModelInputService(
        IMongoClient client,
        IConfiguration config,
        ILogger<MLModelInputService> logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService)
        : BaseService<MLModelInput>(client, config, logger, _environment, _hubContext, _userConnectionService, MLModelInputs), IMLModelInputService
    {
    }
}
