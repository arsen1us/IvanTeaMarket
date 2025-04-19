using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.PersonalUserBidEntity
{
    /// <summary>
    /// Класс для создания персональной заявки пользователя
    /// </summary>
    public class PersonalUserBidAddDto
    {
        /// <summary>
        /// Имя пользователя 
        /// </summary
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Телефон пользователя 
        /// </summary>
        [JsonProperty("phone")]
        public string Phone { get; set; } = null!;

        /// <summary>
        /// Почта пользователя
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Детали заявки
        /// </summary>
        [JsonProperty("details")]
        public string? Details { get; set; } = null;
    }
}
