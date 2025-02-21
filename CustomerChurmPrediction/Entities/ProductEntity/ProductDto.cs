using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.ProductEntity
{
    /// <summary>
    /// Модель для создания продукта 
    /// </summary>
    public class ProductDto
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
        /// Количество
        /// </summary>
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        [JsonProperty("price")]
        public double Price { get; set; }

        /// <summary>
        /// Id компании
        /// </summary>
        [JsonProperty("companyId")]
        public string CompanyId { get; set; } = null!;

        /// <summary>
        /// Изображения
        /// </summary>
        [JsonProperty("images")]
        public IFormFileCollection? Images { get; set; }

        /// <summary>
        /// Id пользователя
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; } = null!;
    }
}

