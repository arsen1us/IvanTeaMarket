﻿using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.OrderEntity
{
    public class Order : AbstractEntity
    {
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
        /// Список позиций в заказе
        /// </summary>
        [JsonProperty("items")]
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
