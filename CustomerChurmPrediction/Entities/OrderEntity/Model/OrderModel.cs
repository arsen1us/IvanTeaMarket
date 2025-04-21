using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CustomerChurmPrediction.Entities.OrderEntity.Model
{
    public class OrderModel
    {
        /// <summary>
        /// Id сущности
        /// </summary>
        [JsonProperty("orderId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Id пользователя
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Цена заказа
        /// </summary>
        [JsonProperty("totalPrice")]
        public double TotalPrice { get; set; }

        /// <summary>
        /// Статус заказа
        /// </summary>
        [JsonProperty("orderStatus")]
        public string OrderStatus { get; set; } = "Created";

        /// <summary>
        /// Элементы заказа
        /// </summary>
        [JsonProperty("items")]
        public List<OrderItemModel> Items { get; set; } = new List<OrderItemModel>();
    }
}
