namespace CustomerChurmPrediction.Entities.CartEntity
{
    public class CartAdd
    {
        /// <summary>
        /// Id продукта
        /// </summary>
        public string ProductId { get; set; } = null!;

        /// <summary>
        /// Id пользователя
        /// </summary>
        public string UserId { get; set; } = null!;
    }
}
