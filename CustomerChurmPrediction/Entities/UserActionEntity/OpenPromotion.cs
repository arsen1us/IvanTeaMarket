namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    /// <summary>
    /// Открыть рекламный пост
    /// </summary>
    public class OpenPromotion : UserAction
    {
        /// <summary>
        /// Id рекламного поста
        /// </summary>
        public string PromotionId { get; set; } = null!;
    }
}
