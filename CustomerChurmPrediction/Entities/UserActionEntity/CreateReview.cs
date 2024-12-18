namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    /// <summary>
    /// Оставить отзыв
    /// </summary>
    public class CreateReview : UserAction
    {
        /// <summary>
        /// Id отзыва
        /// </summary>
        public string? ReviewId { get; set; } = null;
    }
}
