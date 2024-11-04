using CustomerChurmPrediction.Entities;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface IProductService : IBaseService<Product>
    {

    }
    public class ProductService(IMongoClient client, IConfiguration config, ILogger<ProductService> logger) 
        : BaseService<Product>(client, config, logger, Products), IProductService
    {

    }
}
