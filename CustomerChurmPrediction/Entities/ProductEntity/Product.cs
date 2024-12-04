namespace CustomerChurmPrediction.Entities.ProductEntity
{
    public class Product : AbstractEntity
    {
        /// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; } = null!;

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Id категории
        /// </summary>
        public string CategoryId { get; set; } = null!;

        /// <summary>
		/// Id компании
		/// </summary>
		public string CompanyId { get; set; } = null!;

        /// <summary>
		/// Цена
		/// </summary>
		public decimal Price { get; set; }

        /// <summary>
		/// Количество
		/// </summary>
		public int Count { get; set; }

        /// <summary>
		/// Id фотографий
		/// </summary>
        public List<string>? ImageIds { get; set; }

        public DiscountInfo Discount { get; set; }


        // Purchase Data
        public int PurchaseCount { get; set; }
        public double PurchaseFrequency { get; set; }
        public decimal AverageOrderValue { get; set; }
        public string Periodicity { get; set; } // e.g., Daily, Weekly, Monthly
        public DateTime FirstPurchaseDate { get; set; }

        // Cart Interaction Data
        public CartInteraction CartInteraction { get; set; }

        // Wishlist/Favorite Interaction Data
        public FavoriteInteraction WishlistInteraction { get; set; }

        // Engagement Metrics
        public EngagementMetrics EngagementMetrics { get; set; }

        // Customer Interaction Data
        public CustomerFeedback CustomerFeedback { get; set; }

        // Promotional and Ad Interaction Data
        public PromotionData PromotionData { get; set; }

        // Customer Segment Information
        public CustomerSegmentInfo CustomerSegmentInfo { get; set; }
    }

    // Информация о скидке
    public class DiscountInfo
    {
        public decimal DiscountAmount { get; set; }
        public string DiscountType { get; set; } // e.g., Percentage, Flat amount
        public DateTime ValidUntil { get; set; }
    }

    // Взаимодействие с корзиной
    public class CartInteraction
    {
        public int AddedToCartCount { get; set; }
        public int RemovedFromCartCount { get; set; }
        public double CartToPurchaseConversionRate { get; set; }
    }

    // Взаимодействие с избранным
    public class FavoriteInteraction
    {
        public int AddedToFavoriteCount { get; set; }
        public int RemovedFromFavoriteCount { get; set; }
        public double FavoriteToPurchaseConversionRate { get; set; }
    }

    // Отслеживание просмотра страниц, длительность просмотра
    public class EngagementMetrics
    {
        public int PageViews { get; set; }
        public double AverageViewDuration { get; set; }
        public double BounceRate { get; set; }
    }

    // Показатели отзывов
    public class CustomerFeedback
    {
        public int ReviewsCount { get; set; }
        public double AverageRating { get; set; }
        public double ReturnRate { get; set; }
    }

    public class PromotionData
    {
        public int EmailClicks { get; set; }
        public double AdClickRate { get; set; }
        public int CouponUseCount { get; set; }
    }

    public class CustomerSegmentInfo
    {
        public List<string> CustomerTypes { get; set; } // e.g., "Frequent Buyer", "Seasonal Buyer"
        public decimal AverageCustomerLifetimeValue { get; set; }
    }
}

