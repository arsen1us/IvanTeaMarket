namespace CustomerChurmPrediction.Entities.SessionEntity
{
    public class Session : AbstractEntity
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Время начала сессии
        /// </summary>
        public DateTime SessionTimeStart { get; set; }

        /// <summary>
        /// Время окончания сессии
        /// </summary>
        public DateTime SessionTimeEnd { get; set; }
    }
}
