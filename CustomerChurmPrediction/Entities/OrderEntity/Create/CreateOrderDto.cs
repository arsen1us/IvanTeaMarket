using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.OrderEntity.Create
{
    public class CreateOrderDto
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Список позиций в заказе
        /// </summary>
        [JsonProperty("items")]
        public List<OrderItemDto> Items { get; set; }
    }
}
