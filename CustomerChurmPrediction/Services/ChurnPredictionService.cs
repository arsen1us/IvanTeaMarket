using MongoDB.Driver;

namespace CustomerChurmPrediction.Services
{
    public class ChurnPredictionService(IMongoClient client, IConfiguration config, ILogger<CartService> logger, IProductService _productService, IWebHostEnvironment _environment)
    {
    }
}
