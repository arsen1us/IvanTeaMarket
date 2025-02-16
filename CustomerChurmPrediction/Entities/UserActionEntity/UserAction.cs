using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    public class UserAction : AbstractEntity
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Тип действия пользователя 
        /// </summary>
        [JsonProperty("actionType")]
        public string ActionType { get; set; } = null!;

        /// <summary>
        /// Время действия
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime ActionTime { get; set; }

        /// <summary>
        /// Дополнительные параметры
        /// </summary>
        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; set; } = new();
    }
}
