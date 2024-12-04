namespace CustomerChurmPrediction.Entities.ReviewEntity
{
    // Отзыв
    public class Review : AbstractEntity
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Id продукта
        /// </summary>
        public string ProductId { get; set; } = null!;

        /// <summary>
        /// Текст отзыва
        /// </summary>
        public string Text { get; set; } = null!;

        /// <summary>
        /// Оценка от (1 до 5)
        /// </summary>
        public int Grade { get; set; }
    }
}
