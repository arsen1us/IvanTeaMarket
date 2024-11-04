namespace CustomerChurmPrediction.Entities
{
    // Купоны пользователей
    public class Coupon : AbstractEntity
    {
        public bool IsActivated { get; set; } = false;
        
        // (Базовое поле CreateTime - время начала работы купона)
        // Время окончания работы купота 
        public DateTime TimeToEnd { get; set; }
    }
}
