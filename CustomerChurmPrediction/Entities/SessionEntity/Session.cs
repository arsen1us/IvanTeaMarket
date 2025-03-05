namespace CustomerChurmPrediction.Entities.SessionEntity
{
    public class Session : AbstractEntity
    {
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
