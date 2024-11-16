using CustomerChurmPrediction.Entities.ProductEntity;

namespace CustomerChurmPrediction.Entities.UserEntity
{
    public class User : AbstractEntity
    {
        // Имя
        public string FirstName { get; set; } = null!;
        // Фамилия
        public string LastName { get; set; } = null!;
        // Почта
        public string Email { get; set; } = null!;
        // Пароль
        public string Password { get; set; } = null!;
        // День рождения
        public DateTime DateOfBirth { get; set; }

        // Данные о покупках
        public int TotalPurchases { get; set; }
        public decimal TotalSpent { get; set; }
        public List<PurchaseHistory> PurchaseHistory { get; set; } = new();
        public int AverageOrderValue { get; set; }

        // Информация о посещаемости сайта
        public double LoginFrequency { get; set; }
        public TimeSpan AverageSessionDuration { get; set; }
        public int TotalPageViews { get; set; }
        public double BounceRate { get; set; }

        // Взаимодействие с корзиной
        public CartInteraction CartInteraction { get; set; } = new();

        // Взаимодействие со списком избранного
        public FavoriteInteraction WishlistInteraction { get; set; } = new();

        // Предпочтения клиентов
        public List<string> PreferredCategories { get; set; } = new();
        public bool ReceivesPromotions { get; set; }

        // Данные о поведении и сегментации
        public List<string> Segments { get; set; } = new(); // e.g., "High Spender", "Frequent Buyer"
        public decimal LifetimeValue { get; set; }
        public bool IsLikelyToChurn { get; set; }

        // Взаимодействия с рекламой 
        public int AdClicks { get; set; }
        public int CouponUses { get; set; }

        // Олбратная связь от клиентов
        public int ReviewsGiven { get; set; }
        public double AverageRatingGiven { get; set; }

        // Конструктор с использованием полей UserReg
        public User(UserReg userReg)
        {
            FirstName = userReg.FirstName;
            LastName = userReg.LastName;
            Email = userReg.Email;
            Password = userReg.Password;
        }
    }

    public class PurchaseHistory
    {
        public string ProductID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal AmountSpent { get; set; }
        public int Quantity { get; set; }
    }
}
