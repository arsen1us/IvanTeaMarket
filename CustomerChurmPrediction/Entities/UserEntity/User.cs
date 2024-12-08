using CustomerChurmPrediction.Entities.ProductEntity;
using CustomerChurmPrediction.Entities.OrderEntity;

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

        /// <summary>
        /// Ссылка на фото пользователя
        /// </summary>
        // Мб, нужен List<string>, так как пользователь может работать с несколькими компаниями
        //public List<string>? CompanyIds { get; set; } = null;
        public List<string> ImageSrcs { get; set; } = new List<string>();

        /// <summary>
        /// Поле показывает, нужно ли применять меры по удержанию пользователя или нет
        /// </summary>
        public bool IsLikelyToChurn { get; set; }

        // Информация о заказах ============================================================

        /// <summary>
        /// Общее количество заказов
        /// </summary>
        public int TotalOrder { get; set; }

        /// <summary>
        /// Список Id заказов
        /// </summary>
        public List<string> OrderIds { get; set; } = new();

        /// <summary>
        /// Средняя цена заказа
        /// </summary>
        public int AverageOrderValue { get; set; }

        /// <summary>
        /// Частота заказов
        /// </summary>
        public double OrderFrequency { get; set; }

        // Информация о заказах ============================================================


        // Информация о пакупках пользователей =============================================
        /// <summary>
        /// Общее количество купленных товаров
        /// </summary>
        public int TotalPurchases { get; set; }

        /// <summary>
        /// Сколько всего потрачено
        /// </summary>
        public decimal TotalSpent { get; set; }

        /// <summary>
        /// Средняя цена покупки
        /// </summary>
        public decimal AveragePurchase { get; set; }

        /// <summary>
        /// Частота покупок
        /// </summary>
        public double PurchaseFrequency { get; set; }
        // Информация о пакупках пользователей =============================================


        // Информация о регистрации и аутентификации и сессиях пользователей ===============
        /// <summary>
        /// Частота входа на сайт
        /// </summary>
        public double LoginFrequency { get; set; }

        /// <summary>
        /// Среднее время сессии
        /// </summary>
        public TimeSpan AverageSessionDuration { get; set; }

        /// <summary>
        /// Количество попыток регистрации
        /// </summary>
        public int RegistrationTries { get; set; }

        /// <summary>
        /// Количество попыток входа
        /// </summary>
        public int AuthenticationTries { get; set; }
        // Информация о регистрации и аутентификации и сессиях пользователей ===============


        // Взаимодействие со страницами ====================================================
        /// <summary>
        /// Число просмотренных страниц
        /// </summary>
        public int TotalPageViews { get; set; }
        // Взаимодействие со страницами ====================================================


        // Взаимодействие с корзиной =======================================================
        /// <summary>
        /// Информация о взаимодействии с корзиной 
        /// </summary>
        // Возможно надо вынести в отдельную таблицу
        public CartInteraction CartInteraction { get; set; } = new();
        // Взаимодействие с корзиной =======================================================


        // Взаимодействия с рекламой =======================================================
        public int AdClicks { get; set; }

        /// <summary>
        /// Колво рекламы, на которую перешёл пользователь
        /// </summary>
        public int PromotionClicksCount { get; set; }

        /// <summary>
        /// Id рекламы, на которую перешёл пользователь
        /// </summary>
        public List<string> PromotionIdClicksList { get; set; }
        // Взаимодействия с рекламой =======================================================


        // Обратная связь клиентов =========================================================
        /// <summary>
        /// Кол-во отзывов
        /// </summary>
        public int ReviewsCount { get; set; }

        /// <summary>
        /// Средняя оценка отзывов
        /// </summary>
        public double AverageRatingGiven { get; set; }

        /// <summary>
        /// Список отзывов пользователей
        /// </summary>
        public List<string> UserReviewIds { get; set; }
        // Обратная связь клиентов =========================================================


        // Информация о страницах, которые посещал пользователей ===========================
        // Добавить отслеживание информации в период 1 сесии

        /// <summary>
        /// Количество ссылко на которые переходил пользователь
        /// </summary>
        public int PageUrlsCount { get; set; }

        /// <summary>
        /// Ссылки на страницы, которые посещал пользователь
        /// </summary>
        public List<string> PageUrls { get; set; } = new List<string>();

        /// <summary>
        /// Ссылка на страницу
        /// </summary>
        public string Url { get; set; } = null!;

        /// <summary>
        /// Среднее время просмотра страницы
        /// </summary>
        public DateTime AverageViewDuration { get; set; }

        // Информация о страницах, которые посещал пользователей ===========================

        // Данные о поведении и сегментации
        public List<string> Segments { get; set; } = new(); // e.g., "High Spender", "Frequent Buyer"
        public decimal LifetimeValue { get; set; }

        // Конструктор с использованием полей UserReg
        public User(UserReg userReg)
        {
            FirstName = userReg.FirstName;
            LastName = userReg.LastName;
            Email = userReg.Email;
            Password = userReg.Password;
        }
    }
}
