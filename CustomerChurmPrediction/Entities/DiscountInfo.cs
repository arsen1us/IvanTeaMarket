namespace CustomerChurmPrediction.Entities
{
    /// <summary>
    /// Информация о скидке
    /// </summary>
    public class DiscountInfo
    {
        /// <summary>
        /// Id продукта, на который действует скидка
        /// </summary>
        public string ProductId { get; set; } = null!;

        /// <summary>
        /// Цена скидки
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Дата начала скидки
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания скидки
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
