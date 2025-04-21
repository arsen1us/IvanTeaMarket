using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.OrderEntity.Create
{
    public class OrderItemDto
    {
        /// <summary>
        /// Id продукта
        /// </summary>
        [JsonProperty("teaId")]
        public string teaId { get; set; } = null!;

        /// <summary>
        /// Количество
        /// </summary>
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
