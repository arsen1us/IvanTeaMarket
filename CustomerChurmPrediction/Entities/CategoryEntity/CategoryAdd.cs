using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.CategoryEntity
{
    /// <summary>
    /// Класс для добавления категории чая
    /// </summary>
	public class CategoryAdd
	{
        /// <summary>
        /// Название категории чая
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = null!;
	}
}
