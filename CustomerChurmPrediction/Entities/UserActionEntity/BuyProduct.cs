namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    /// <summary>
    /// Купить продукт
    /// </summary>
    public class BuyProduct : UserAction
    {
        /// <summary>
        /// Id продукта
        /// </summary>
        public string? ProductId { get; set; } = null;   
    }
}
