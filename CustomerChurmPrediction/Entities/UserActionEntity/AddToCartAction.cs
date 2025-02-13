namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    /// <summary>
    /// Действие добавление товара в корзину 
    /// </summary>
    public class AddToCartAction : UserAction
    {
        public string ProductId { get; set; } = null!;
    }
}
