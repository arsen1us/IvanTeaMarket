using CustomerChurmPrediction.Entities.CartEntity;
using CustomerChurmPrediction.Entities.PersonalUserBidEntity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface IPersonalUserBidService : IBaseService<PersonalUserBid>
    {

    }
    public class PersonalUserBidService(
        IMongoClient client,
        IConfiguration config,
        ILogger<PersonalUserBidService> logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService)
        : BaseService<PersonalUserBid>(client, config, logger, _environment, _hubContext, _userConnectionService, PersonalUserBids), IPersonalUserBidService
    {
    }
}
