using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.OrderEntity.Create
{
    public class OrderItemDto
    {
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
    }
}
