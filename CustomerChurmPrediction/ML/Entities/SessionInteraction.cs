namespace CustomerChurmPrediction.ML.Entities
{
    /// <summary>
    /// Информация о сессиях пользователя
    /// </summary>
    public class SessionInteraction
    {
        /// <summary>
        /// Общее количество сессий
        /// </summary>
        public int TotalSessions { get; set; }

        /// <summary>
        /// Средняя длительность сессии (в минутах)
        /// </summary>
        public double AvgSessionDuration { get; set; }

        /// <summary>
        /// Максимальная длительность сессии (в минутах)
        /// </summary>
        public double MaxSessionDuration { get; set; }

        /// <summary>
        /// Средний интервал между сессиями (в минутах)
        /// </summary>
        public double AvgTimeBetweenSessions { get; set; }

        /// <summary>
        /// Время с последнего визита (в минутах)
        /// </summary>
        public double TimeSinceLastSession { get; set; }
    }
}
