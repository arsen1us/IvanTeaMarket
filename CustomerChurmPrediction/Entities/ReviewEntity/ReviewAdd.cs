namespace CustomerChurmPrediction.Entities.ReviewEntity
{
    /// <summary>
    /// Класс для создания отзыва
    /// </summary>
    public class ReviewAdd
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Id чая
        /// </summary>
        public string TeaId { get; set; } = null!;

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
