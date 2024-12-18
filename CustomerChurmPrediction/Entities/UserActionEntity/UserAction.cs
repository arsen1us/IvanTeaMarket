using CustomerChurmPrediction.Utils;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes( typeof(AddToCart), 
        typeof(AuthenticationAttempt),
        typeof(AuthenticationSuccess),
        typeof(BuyProduct),
        typeof(BuyProductFromCart),
        typeof(CreateReview),
        typeof(OpenPage),
        typeof(RegistrationAttempt),
        typeof(RegistrationSuccess),
        typeof(OpenPage),
        typeof(UseCoupon))]
    public class UserAction : AbstractEntity
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Время действия пользователя
        /// </summary>
        public DateTime ActionDateTime { get; set; }
    }
}
