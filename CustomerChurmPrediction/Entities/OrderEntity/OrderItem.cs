using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.OrderEntity
{
    public class OrderItem : AbstractEntity
    {
        /// <summary>
        /// Id заказа
        /// </summary>
        [JsonProperty("orderId")]
        public string OrderId { get; set; } = null!;

        /// <summary>
        /// Id продукта
        /// </summary>
        [JsonProperty("productId")]
        public string ProductId { get; set; } = null!;

        /// <summary>
        /// Количество
        /// </summary>
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Цена за 1 единицу продукта на момент покупки
        /// </summary>
        [JsonProperty("unitPrice")]
        public double UnitPrice { get; set; }

        /// <summary>
        /// Общая стоимость данной позиции
        /// </summary>
        [JsonProperty("totalPrice")]
        public double TotalPrice { get; set; }
    }
}
