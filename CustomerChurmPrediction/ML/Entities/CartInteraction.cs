namespace CustomerChurmPrediction.ML.Entities
{
    /// <summary>
    /// Взаимодействие с корзиной
    /// </summary>
    public class CartInteraction
    {
        /// <summary>
        /// Общее количество добавлений в корзину
        /// </summary>
        public int TotalCartAdds { get; set; }

        /// <summary>
        /// Среднее время между добавлениями (минуты)
        /// </summary>
        public double AvgTimeBetweenAdds { get; set; }

        /// <summary>
        /// Максимальное время между добавлениями (минуты)
        /// </summary>
        public double MaxTimeBetweenAdds { get; set; }

        /// <summary>
        /// Время с последнего добавления
        /// </summary>
        public double TimeSinceLastAdd { get; set; }

        /// <summary>
        /// Уникальные товары в корзине
        /// </summary>
        public int UniqueProducts { get; set; }
    }
}
