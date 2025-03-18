using CustomerChurmPrediction.Entities.UserEntity;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CustomerChurmPrediction.ML.Entities.ChurnPredictionEntity
{
    public class ChurnPredictionModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

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
