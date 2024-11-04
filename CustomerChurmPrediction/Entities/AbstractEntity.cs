using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Entities
{
    public class AbstractEntity
    {
        [BsonId]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        // Время обновления сущности
        public DateTime LastTimeUserUpdate { get; set; }
        
        // Время создания сущности
        public DateTime CreateTime { get; set; }
    }
}
