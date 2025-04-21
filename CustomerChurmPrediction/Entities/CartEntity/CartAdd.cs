using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.CartEntity
{
    public class CartAdd
    {
        /// <summary>
        /// Id продукта
        /// </summary>
        [JsonProperty("teaId")]
        public string TeaId { get; set; } = null!;

        /// <summary>
        /// Id пользователя
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; } = null!;
    }
}
