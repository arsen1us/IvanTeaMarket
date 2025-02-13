namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    /// <summary>
    /// Действие создание отзыва на продукт
    /// </summary>
    public class CreateReviewAction : UserAction
    {
        /// <summary>
        /// Id отзыва
        /// </summary>
        public string? ReviewId { get; set; } = null;
    }
}
