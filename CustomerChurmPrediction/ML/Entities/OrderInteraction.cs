namespace CustomerChurmPrediction.ML.Entities
{
    /// <summary>
    /// Взаимодействие с заказами
    /// </summary>
    public class OrderInteraction
    {
        /// <summary>
        /// Общее число заказов
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// Средняя стоимость заказа
        /// </summary>
        public double AvgOrderPrice { get; set; }

        /// <summary>
        /// Максимальная стоимость заказа
        /// </summary>
        public double MaxOrderPrice { get; set; }

        /// <summary>
        /// Среднее число позиций в заказе
        /// </summary>
        public double AvgItemsPerOrder { get; set; }

        /// <summary>
        /// Количество уникальных товаров
        /// </summary>
        public int UniqueProducts { get; set; }

        /// <summary>
        /// Время с последнего заказа (в минутах)
        /// </summary>
        public double MinutesSinceLastOrder { get; set; }

        /// <summary>
        /// Средний интервал между заказами (в минутах)
        /// </summary>
        public double AvgMinutesBetweenOrders { get; set; }

        /// <summary>
        /// Количество отменённых заказов
        /// </summary>
        public int CancelledOrders { get; set; }
    }
}
