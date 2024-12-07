using CustomerChurmPrediction.Entities.CouponEntity;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface ICouponService : IBaseService<Coupon>
    {

    }
    public class CouponService(IMongoClient client, IConfiguration config, ILogger<CouponService> logger, IWebHostEnvironment _environment) 
        : BaseService<Coupon>(client, config, logger, _environment, Coupons), ICouponService
    {

    }
}
