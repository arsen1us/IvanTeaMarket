namespace CustomerChurmPrediction.Entities.OrderEntity
{
    public class Order : AbstractEntity
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
        /// Цена заказа
        /// </summary>
        public decimal TotalPrice { get; set; }
        public Order() { }

        public Order(string productId, string companyId, int productCount, string userId, decimal totalPrice)
        {
            ProductId = productId;
            ProductCount = productCount;
            CompanyId = companyId;
            UserId = userId;
            TotalPrice = totalPrice;
        }
    }
}
