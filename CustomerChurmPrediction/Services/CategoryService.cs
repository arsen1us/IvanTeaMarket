using CustomerChurmPrediction.Entities.CategoryEntity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface ICategoryService : IBaseService<Category>
    {

    }
    public class CategoryService(
        IMongoClient client, 
        IConfiguration config,
        ILogger<CategoryService> logger,
        IWebHostEnvironment _environment, 
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService)
        : BaseService<Category>(client, config, logger, _environment, _hubContext, _userConnectionService, Categories), ICategoryService
    {
    }
}
