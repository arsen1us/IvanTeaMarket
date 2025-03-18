namespace CustomerChurmPrediction.ML.Entities
{
    /// <summary>
    /// Статистика действий пользователя
    /// </summary>
    public class UserActionStat
    {
        /// <summary>
        /// Общее количество действий
        /// </summary>
        public int TotalActions { get; set; }

        /// <summary>
        /// Количество действий по типу
        /// </summary>
        public Dictionary<string, int> ActionsByType { get; set; } = new();

        /// <summary>
        /// Средний интервал между действиями (в минутах)
        /// </summary>
        public double AvgTimeBetweenActions { get; set; }

        /// <summary>
        /// Максимальный интервал между действиями (в минутах)
        /// </summary>
        public double MaxTimeBetweenActions { get; set; }

        /// <summary>
        /// Время с последнего действия (в минутах)
        /// </summary>
        public double TimeSinceLastAction { get; set; }
    }
}
