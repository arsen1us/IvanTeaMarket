namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    /// <summary>
    /// Действие открытия рекламного поста
    /// </summary>
    public class OpenPromotionAction : UserAction
    {
        /// <summary>
        /// Id рекламного поста
        /// </summary>
        public string PromotionId { get; set; } = null!;
    }
}
