namespace CustomerChurmPrediction.Entities.OrderEntity
{
    public class OrderItemDto
    {
        /// <summary>
        /// Id продукта
        /// </summary>
        public string ProductId { get; set; } = null!;

        /// <summary>
        /// Количество
        /// </summary>
        public int Quantity { get; set; }
    }

    public class OrderListDto
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Список товаров в заказе
        /// </summary>
        public List<OrderItemDto> OrderList { get; set; } = new();
    }
}
