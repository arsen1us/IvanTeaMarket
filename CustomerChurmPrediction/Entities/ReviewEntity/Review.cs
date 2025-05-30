﻿using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.ReviewEntity
{
    // Отзыв
    public class Review : AbstractEntity
    {
        /// <summary>
        /// Id чая
        /// </summary>
        [JsonProperty("teaId")]
        public string TeaId { get; set; } = null!;

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
