namespace CustomerChurmPrediction.Utils
{
    public static class UserRoles
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public static readonly string User = "User";

        /// <summary>
        /// Администатор
        /// </summary>
        public static readonly string Admin = "Admin";

        /// <summary>
        /// Владелец
        /// </summary>
        public static readonly string Owner = "Owner";
    }

    public static class CollectionName
    {
        /// <summary>
        /// Корзина
        /// </summary>
        public readonly static string Carts = "Carts";

        /// <summary>
        /// Категории
        /// </summary>
        public readonly static string Categories = "Categories";

        /// <summary>
        /// Избранное
        /// </summary>
        public readonly static string Favorites = "Favorites";

        /// <summary>
        /// Пользователи
        /// </summary>
        public readonly static string Users = "Users";

        /// <summary>
        /// Товары
        /// </summary>
        public readonly static string Products = "Products";

        /// <summary>
        /// Купоны
        /// </summary>
        public readonly static string Coupons = "Coupons";

        /// <summary>
        /// Реклама
        /// </summary>
        public readonly static string Promotions = "Promotions";

        /// <summary>
        /// Отзывы
        /// </summary>
        public readonly static string Reviews = "Reviews";

        /// <summary>
        /// Вкладки сайта (страницы с товарами)
        /// </summary>
        public readonly static string Pages = "Pages";

        /// <summary>
        /// Компании
        /// </summary>
        public readonly static string Companies = "Companies";

        /// <summary>
        /// Заказы
        /// </summary>
        public readonly static string Orders = "Orders";

        /// <summary>
        /// Сессии пользователей
        /// </summary>
        public readonly static string Sessions = "Sessions";

        /// <summary>
        /// Действия пользователей
        /// </summary>
        public readonly static string UserActions = "UserActions";

        /// <summary>
        /// Уведомления
        /// </summary>
        public readonly static string Notifications = "Notifications";

        /// <summary>
        /// Предсказания оттока
        /// </summary>
        public readonly static string ChurnPredictions = "ChurnPredictions";

        /// <summary>
        /// Информация о пользователе
        /// </summary>
        public readonly static string UsersInformation = "UsersInformation";

        /// <summary>
        /// Коллекция MLModelInputs
        /// </summary>
        public readonly static string MLModelInputs = "MLModelInputs";
    }

    /// <summary>
    /// Название методов для сервиса уведомлений SignalR
    /// </summary>
    public static class SignalRMethods
    {
        /// <summary>
        /// Уведомление при подключении
        /// </summary>
        public readonly static string OnConnected = "OnConnected";

        /// <summary>
        /// Уведомление после успешной операции в бд
        /// </summary>
        public readonly static string SendDatabaseNotification = "SendDatabaseNotification";

        public readonly static string SendForAll = "SendForAll";
    }
}

    
