using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;
using CustomerChurmPrediction.Entities.PageEntity;

namespace CustomerChurmPrediction.Services
{
    public interface IPageService : IBaseService<Page>
    {
        public Task<bool> IsPageViewedAsync(string userId, string pageId);
    }

    public class PageService(IMongoClient client, IConfiguration config, ILogger<Page> logger)
        : BaseService<Page>(client, config, logger, Pages), IPageService
    {
        // Проверить, просмотрена ли страница данным пользователем или нет
        public async Task<bool> IsPageViewedAsync(string userId, string pageId)
        {
            var filter = Builders<Page>.Filter.And(
                Builders<Page>.Filter.Eq(p => p.UserId, userId),
                Builders<Page>.Filter.Eq(p => p.PageUrl, pageId));

            var result = await base.FindAllAsync(filter);
            if (result.Any()) 
                return true;
            return false;
        }

    }
}
