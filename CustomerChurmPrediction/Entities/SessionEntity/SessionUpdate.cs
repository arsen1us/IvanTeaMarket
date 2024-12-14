namespace CustomerChurmPrediction.Entities.SessionEntity
{
    public class SessionUpdate
    {
        /// <summary>
        /// Id пользователя 
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Время действия пользователя в текущей сессии
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
