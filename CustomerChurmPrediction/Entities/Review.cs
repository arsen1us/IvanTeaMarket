namespace CustomerChurmPrediction.Entities
{
    // Отзыв
    public class Review : AbstractEntity
    {
        public string UserId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        // Текст отзыва
        public string Text { get; set; } = null!;

        // Оценка от 1 до 5
        public int Grade { get; set; }
    }
}
