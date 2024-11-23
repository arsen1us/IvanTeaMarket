using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Entities
{
    public class AbstractEntity
    {
        /// <summary>
        /// Id сущности
        /// </summary>
        [BsonId]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        /// <summary>
        /// Время обновления сущности
        /// </summary>
        public DateTime LastTimeUserUpdate { get; set; } = DateTime.Now;

        /// <summary>
        /// Время создания сущности
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
