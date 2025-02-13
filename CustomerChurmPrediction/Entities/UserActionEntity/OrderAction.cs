namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    /// <summary>
    /// Действие создания заказа
    /// </summary>
    public class OrderAction : UserAction
    {
        /// <summary>
        /// Id заказа
        /// </summary>
        public string? OrderId { get; set; } = null;   
    }
}
