using CustomerChurmPrediction.Entities.ProductEntity;

namespace CustomerChurmPrediction.Entities.UserEntity
{
    public class User : AbstractEntity
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// День рождения
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public string Role { get; set; } = null!;

        /// <summary>
        /// Id компании, с которой может работать пользователь (на основе роли)
        /// </summary>
        public string? CompanyId { get; set; } = null;
        // Мб, нужен List<string>, так как пользователь может работать с несколькими компаниями
        //public List<string>? CompanyIds { get; set; } = null;

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
