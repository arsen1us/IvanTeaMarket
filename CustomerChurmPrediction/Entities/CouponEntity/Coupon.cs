using Amazon.Util;

namespace CustomerChurmPrediction.Entities.CouponEntity
{
    // Купоны пользователей
    public class Coupon : AbstractEntity
    {
        /// <summary>
        /// Активирован ли купол или нет
        /// </summary>
        public bool IsActivated { get; set; } = false;

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

        /// <summary>
        /// Дата начала действия купона
        /// </summary>
        public DateTime StardDate { get; set; }

        /// <summary>
        /// Дата окончания действия купона
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Время начала
        /// </summary>
        public TimeOnly StartTime { get; set; }

        /// <summary>
        /// Время окончания
        /// </summary>
        public TimeOnly EndTime { get; set; }
        public Coupon()
        {

        }
    }
}
