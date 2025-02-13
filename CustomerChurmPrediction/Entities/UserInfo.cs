using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CustomerChurmPrediction.Entities
{
    public class UserInfo : AbstractEntity
    {
        /// <summary>
        /// Id пользователя 
        /// </summary>
        public string UserId { get; set; } = null!;
    }

    /// <summary>
    /// Взаимодействие пользователя с скорзиной
    /// </summary>
    public class CartInteractionInfo
    {

    }


}
