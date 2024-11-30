namespace CustomerChurmPrediction.Entities.CouponEntity
{
    public class CouponUpdate : AbstractEntity
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
        /// Id категорий, на которые будет распространяться купон
        /// </summary>
        public List<string> CategoriesIds { get; set; } = new List<string>();

        /// <summary>
        /// Id компании
        /// </summary>
        public string CompanyId { get; set; } = null!;
    }
}
