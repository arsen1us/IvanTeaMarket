using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.PageEntity
{
    public class PageAdd
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Ссылка на страницу
        /// </summary>
        [JsonProperty("pageUrl")]
        public string PageUrl { get; set; } = null!;
    }
}
