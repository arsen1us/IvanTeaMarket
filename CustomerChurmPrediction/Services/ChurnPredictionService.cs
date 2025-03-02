using MongoDB.Driver;
using CustomerChurmPrediction.Entities;

namespace CustomerChurmPrediction.Services
{
    public interface IChurnPredictionService : IBaseService<ChurnPrediction> { }

    /// <summary>
    /// Сервис для работы с предсказаниями оттока пользователей
    /// </summary>
    /// <param name="client"></param>
    /// <param name="config"></param>
    /// <param name="logger"></param>
    /// <param name="_productService"></param>
    /// <param name="_environment"></param>
    public class ChurnPredictionService(
        IMongoClient client,
        IConfiguration config,
        ILogger<CartService> logger,
        IProductService _productService,
        IWebHostEnvironment _environment) { }
}
