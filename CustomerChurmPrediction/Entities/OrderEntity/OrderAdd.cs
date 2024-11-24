namespace CustomerChurmPrediction.Entities.OrderEntity
{
    public class OrderAdd
    {
        /// <summary>
        /// Id продукта
        /// </summary>
        public string ProductId { get; set; } = null!;

        /// <summary>
        /// Id пользователя
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// Id компании
        /// </summary>
        public string CompanyId { get; set; } = null!;

        /// <summary>
        /// Id пользователя
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Цена 1 продукта (будет конвертирована при оформлеции заказа - Price * ProductCount)
        /// </summary>
        public decimal Price { get; set; }
        public OrderAdd() { }

        public OrderAdd(string productId, string companyId, int productCount, string userId, decimal price)
        {
            ProductId = productId;
            ProductCount = productCount;
            CompanyId = companyId;
            UserId = userId;
            Price = price;
        }
    }
}
