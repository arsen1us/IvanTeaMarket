namespace CustomerChurmPrediction.ML.Entities
{
    /// <summary>
    /// Взаимодействие со страницами
    /// </summary>
    public class PageInteraction
    {
        /// <summary>
        /// Общее количество посещений страниц
        /// </summary>
        public int TotalPageViews { get; set; }

        /// <summary>
        /// Количество уникальных страниц
        /// </summary>
        public int UniquePages { get; set; }

        /// <summary>
        /// Среднее время между просмотрами страниц (в минутах)
        /// </summary>
        public double AvgTimeBetweenViews { get; set; }

        /// <summary>
        /// Максимальное время между просмотрами страниц (в минутах)
        /// </summary>
        public double MaxTimeBetweenViews { get; set; }

        /// <summary>
        /// Время с последнего просмотра (в минутах)
        /// </summary>
        public double TimeSinceLastView { get; set; }
    }
}
