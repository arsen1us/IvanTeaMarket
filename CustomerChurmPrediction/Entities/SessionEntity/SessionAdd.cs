namespace CustomerChurmPrediction.Entities.SessionEntity
{
    public class SessionAdd
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Время начала сессии
        /// </summary>
        public DateTime SessionTimeStart { get; set; }
    }
}
