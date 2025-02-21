using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;
using CustomerChurmPrediction.Entities.PageEntity;
using Microsoft.AspNetCore.SignalR;

namespace CustomerChurmPrediction.Services
{
    public interface IPageService : IBaseService<Page>
    {
    }

    public class PageService(
        IMongoClient client,
        IConfiguration config,
        ILogger<Page> logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService)
        : BaseService<Page>(client, config, logger, _environment, _hubContext, _userConnectionService, Pages), IPageService
    {
    }
}
