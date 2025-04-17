namespace CustomerChurmPrediction.Entities.PersonalUserBidEntity
{
    /// <summary>
    /// Класс для обновления персональной заявки пользователя
    /// </summary>
    public class PersonalUserBidUpdateDto
    {
        /// <summary>
        /// Имя пользователя 
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Телефон пользователя 
        /// </summary>
        public string Phone { get; set; } = null!;

        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Детали заявки
        /// </summary>
        public string? Details { get; set; } = null;
    }
}
