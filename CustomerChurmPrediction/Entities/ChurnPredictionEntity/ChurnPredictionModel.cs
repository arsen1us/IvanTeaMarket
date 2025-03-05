using CustomerChurmPrediction.Entities.UserEntity;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CustomerChurmPrediction.Entities.ChurnPredictionEntity
{
    public class ChurnPredictionModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// Предсказание по оттоку 
        /// </summary>
        public ChurnPrediction? ChurnPrediction { get; set; }
    }
}
