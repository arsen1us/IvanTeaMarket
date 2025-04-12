using CustomerChurmPrediction.Entities.TeaEntity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface ITeaService : IBaseService<Tea>
    {

    }

    public class TeaService(
        IMongoClient client,
        IConfiguration config,
        ILogger<TeaService> logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService) : BaseService<Tea>(client, config, logger, _environment, _hubContext, _userConnectionService, Teas), ITeaService
    {
    }
}
