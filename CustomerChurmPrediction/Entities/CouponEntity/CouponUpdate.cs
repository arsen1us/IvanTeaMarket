namespace CustomerChurmPrediction.Entities.CouponEntity
{
    public class CouponUpdate : AbstractEntity
    {
        public bool IsActivated { get; set; } = false;

        public string Key { get; set; } = null!;

        // Id товаров, на которые будет распространяться купон
        public List<string> ProductIds { get; set; } = new List<string>();

        // Id категорий, на которые будет распространяться купон
        public List<string> CategoriesIds { get; set; } = new List<string>();

        // (Базовое поле CreateTime - время начала работы купона)
        // Время окончания работы купота 
        public DateTime TimeToEnd { get; set; }
    }
}
