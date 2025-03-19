using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities
{
    /// <summary>
    /// Лог, приходящий с клиента React
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Тип лога
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = null!;

        /// <summary>
        /// Сообщение лога
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; } = null!;
    }
}
