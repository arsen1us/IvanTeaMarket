namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    /// <summary>
    /// Купить продукт из корзины
    /// </summary>
    public class BuyProductFromCart : UserAction
    {
        /// <summary>
        /// Id продукта
        /// </summary>
        public string? ProductId { get; set; } = null;

        /// <summary>
        /// Id корзины
        /// </summary>
        public string? CartId { get; set; } = null;
    }
}
