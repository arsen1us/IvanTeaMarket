using CustomerChurmPrediction.Entities.CouponEntity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface ICouponService : IBaseService<Coupon>
    {

    }
    public class CouponService(
        IMongoClient client,
        IConfiguration config,
        ILogger<CouponService> logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService) 
        : BaseService<Coupon>(client, config, logger, _environment, _hubContext, _userConnectionService, Coupons), ICouponService
    {

    }
}
