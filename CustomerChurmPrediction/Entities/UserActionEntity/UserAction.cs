using CustomerChurmPrediction.Utils;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes( typeof(AddToCartAction), 
        typeof(AuthenticateAction),
        typeof(OrderAction),
        typeof(CreateReviewAction),
        typeof(OpenPageAction),
        typeof(RegisterAction),
        typeof(OpenPageAction),
        typeof(UseCouponAction))]
    public class UserAction : AbstractEntity
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Время действия
        /// </summary>
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
