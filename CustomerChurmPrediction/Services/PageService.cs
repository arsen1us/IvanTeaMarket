using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;
using CustomerChurmPrediction.Entities.PageEntity;

namespace CustomerChurmPrediction.Services
{
    public interface IPageService : IBaseService<Page>
    {
    }

    public class PageService(IMongoClient client, IConfiguration config, ILogger<Page> logger, IWebHostEnvironment _environment)
        : BaseService<Page>(client, config, logger, _environment, Pages), IPageService
    {
    }
}
