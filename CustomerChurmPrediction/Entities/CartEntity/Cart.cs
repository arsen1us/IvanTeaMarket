using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.CartEntity
{
    public class Cart : AbstractEntity
    {
        /// <summary>
        /// Id продукта
        /// </summary>
        [JsonProperty("teaId")]
        public string TeaId { get; set; } = null!;
    }
}
