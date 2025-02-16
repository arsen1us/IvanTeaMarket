using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.ProductEntity
{
    public class Product : AbstractEntity
    {
        /// <summary>
		/// Название
		/// </summary>
        [JsonProperty("name")]
		public string Name { get; set; } = null!;

        /// <summary>
        /// Описание
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; } = null!;

        /// <summary>
        /// Id категории
        /// </summary>
        [JsonProperty("categoryId")]
        public string CategoryId { get; set; } = null!;

        /// <summary>
		/// Id компании
		/// </summary>
        [JsonProperty("companyId")]
        public string CompanyId { get; set; } = null!;

        /// <summary>
		/// Цена
		/// </summary>
        [JsonProperty("price")]
        public double Price { get; set; }

        /// <summary>
		/// Количество
		/// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
		/// Id фотографий
		/// </summary>
        [JsonProperty("imageSrcs")]
        public List<string>? ImageSrcs { get; set; }

        /// <summary>
        /// Информация о скидке
        /// </summary>
        [JsonProperty("discount")]
        public DiscountInfo? Discount { get; set; }
    }
}

