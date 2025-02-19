using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.CompanyEntity
{
    public class CompanyAdd
    {
        /// <summary>
        /// Название компании
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Описание компании
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; } = null!;

        /// <summary>
        /// Id пользователя
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; } = null!;

        /// <summary>
		/// Аватар компании
		/// </summary>
        [JsonProperty("images")]
        public IFormFileCollection? Images { get; set; }
    }
}
