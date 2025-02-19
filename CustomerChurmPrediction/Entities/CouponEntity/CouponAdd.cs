using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.CouponEntity
{
	public class CouponAdd
	{
        /// <summary>
        /// Ключ активации
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; } = null!;

        /// <summary>
        /// Процент скидки на продукт
        /// </summary>
        [JsonProperty("discountPercentage")]
        public double DiscountPercentage { get; set; }

        /// <summary>
        /// Id компании
        /// </summary>
        [JsonProperty("companyId")]
        public string CompanyId { get; set; } = null!;

        /// <summary>
        /// Дата окончания купона 
        /// </summary>
        [JsonProperty("expirationDate")]
        public string ExpirationDate { get; set; } = null!;

    }
}
