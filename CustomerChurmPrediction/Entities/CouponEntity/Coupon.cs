using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.CouponEntity
{
    /// <summary>
    /// Купон (данный купон применяетсяс ко всем продуктам компании)
    /// </summary>
    public class Coupon : AbstractEntity
    {
        /// <summary>
        /// Ключ активации
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; } = null!;

        /// <summary>
        /// Активен ли купон
        /// </summary>
        [JsonProperty("isActive")]
        public bool IsActive { get; set; } = false;

        /// <summary>
        /// Процент скидки на продукт
        /// </summary>
        [JsonProperty("discountPercentage")]
        public double DiscountPercentage {get; set;}

        /// <summary>
        /// Id компании
        /// </summary>
        [JsonProperty("companyId")]
        public string CompanyId { get; set; } = null!;

        /// <summary>
        /// Дата окончания купона 
        /// </summary>
        [JsonProperty("expirationDate")]
        public DateTime ExpirationDate { get; set; }
    }
}
