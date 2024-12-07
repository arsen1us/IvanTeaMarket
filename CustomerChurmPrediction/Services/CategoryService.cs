using CustomerChurmPrediction.Entities.CategoryEntity;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface ICategoryService : IBaseService<Category>
    {

    }
    public class CategoryService(IMongoClient client, IConfiguration config, ILogger<CategoryService> logger, IWebHostEnvironment _environment)
        : BaseService<Category>(client, config, logger, _environment, Categories), ICategoryService
    {
    }
}
