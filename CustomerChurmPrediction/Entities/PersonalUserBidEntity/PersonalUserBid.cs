namespace CustomerChurmPrediction.Entities.PersonalUserBidEntity
{
    /// <summary>
    /// Персональная заявка пользователя
    /// </summary>
    public class PersonalUserBid : AbstractEntity
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
