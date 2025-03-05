using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.ReviewEntity
{
    // Отзыв
    public class Review : AbstractEntity
    {
        /// <summary>
        /// Id продукта
        /// </summary>
        [JsonProperty("productId")]
        public string ProductId { get; set; } = null!;

        /// <summary>
        /// Текст отзыва
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; } = null!;

        /// <summary>
        /// Оценка от (1 до 5)
        /// </summary>
        [JsonProperty("grade")]
        public int Grade { get; set; }
    }
}
