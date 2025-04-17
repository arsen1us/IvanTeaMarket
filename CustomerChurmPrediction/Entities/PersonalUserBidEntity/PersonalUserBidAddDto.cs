namespace CustomerChurmPrediction.Entities.PersonalUserBidEntity
{
    /// <summary>
    /// Класс для создания персональной заявки пользователя
    /// </summary>
    public class PersonalUserBidAddDto
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
