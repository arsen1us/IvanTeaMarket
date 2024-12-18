using CustomerChurmPrediction.Entities;

namespace CustomerChurmPrediction.ML.Entities
{
    /// <summary>
    /// Данные пользователей для обучения модели
    /// </summary>
    public class UserData : AbstractEntity
    {
        /// <summary>
        /// Общее число заказов
        /// </summary>
        public float TotalOrder { get; set; }
        /// <summary>
        /// Общее число покупок
        /// </summary>
        public float TotalPurchases { get; set; }
        /// <summary>
        /// Всего потрачено
        /// </summary>
        public float TotalSpent { get; set; }

        /// <summary>
        /// Количество кликов на рекламы
        /// </summary>
        public float AdClicks { get; set; }

        /// <summary>
        /// Количество использованных купонов
        /// </summary>
        public int UseCoupons { get; set; }

        /// <summary>
        /// Частота входа
        /// </summary>
        public float LoginFrequency { get; set; }

        /// <summary>
        /// Среднее время сессии
        /// </summary>
        public float AverageSessionDuration { get; set; }

        /// <summary>
        /// Вохможен ли уход с сайта или нет
        /// </summary>
        public bool IsLikelyToChurn { get; set; } 
    }
}
