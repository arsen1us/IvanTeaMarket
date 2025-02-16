using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.OrderEntity.Model
{
    public class OrderItemModel
    {
        /// <summary>
        /// Id продукта
        /// </summary>
        [JsonProperty("productId")]
        public string ProductId { get; set; } = null!;

        /// <summary>
        /// Название продукта
        /// </summary>
        [JsonProperty("productName")]
        public string ProductName { get; set; } = null!;

        /// <summary>
        /// Ссылка на изображение продукта
        /// </summary>
        [JsonProperty("productImageUrl")]
        public string ProductImageUrl { get; set; } = null!;

        /// <summary>
        /// Количество
        /// </summary>
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Цена за 1 единицу продукта
        /// </summary>
        [JsonProperty("unitPrice")]
        public double UnitPrice { get; set; }

        /// <summary>
        /// Общая стоимость
        /// </summary>
        [JsonProperty("totalPrice")]
        public double TotalPrice { get; set; }
    }
}
