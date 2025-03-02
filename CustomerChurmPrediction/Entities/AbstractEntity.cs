﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CustomerChurmPrediction.Entities
{
    public class AbstractEntity
    {
        /// <summary>
        /// Id сущности
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        /// <summary>
        /// Время обновления сущности
        /// </summary>
        public DateTime? LastTimeUserUpdate { get; set; } = null;

        /// <summary>
        /// Время создания сущности
        /// </summary>
        public DateTime? CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Id создателя
        /// </summary>
        public string? CreatorId { get; set; }

        /// <summary>
        /// Id пользователя, последнего изменившего запись
        /// </summary>
        public string? UserIdLastUpdate { get; set; }
    }
}
