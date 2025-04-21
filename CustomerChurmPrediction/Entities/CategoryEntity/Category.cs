using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.CategoryEntity
{
    /// <summary>
    /// Категория чая
    /// </summary>
    public class Category : AbstractEntity
    {
        /// <summary>
        /// Название категории чая
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = null!;
    }
}
