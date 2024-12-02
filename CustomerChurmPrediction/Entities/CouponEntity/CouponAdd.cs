namespace CustomerChurmPrediction.Entities.CouponEntity
{
	public class CouponAdd : AbstractEntity
	{
        /// <summary>
        /// Ключ активации
        /// </summary>
        public string Key { get; set; } = null!;

        /// <summary>
        /// Id продуктов, на которые будет распространяться купон
        /// </summary>
        public List<string> ProductIds { get; set; } = new List<string>();

        /// <summary>
        /// Id компании
        /// </summary>
        public string CompanyId { get; set; } = null!;

        /// <summary>
        /// Дата начала действия купона
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания действия купона
        /// </summary>
        public DateTime EndDate { get; set; }

    }
}
