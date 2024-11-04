using CustomerChurmPrediction.Entities;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface ICouponService : IBaseService<Coupon>
    {

    }
    public class CouponService(IMongoClient client, IConfiguration config, ILogger<CouponService> logger) 
        : BaseService<Coupon>(client, config, logger, Coupons), ICouponService
    {

    }
}
