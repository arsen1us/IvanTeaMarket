namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    /// <summary>
    /// Добавить товар в корзину 
    /// </summary>
    public class AddToCart : UserAction
    {
        public string ProductId { get; set; } = null!;
    }
}
