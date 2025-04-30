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
        /// Коллекция с информацией о корзинах пользователей
        /// </summary>
        public readonly static string Carts = "Carts";

        /// <summary>
        /// Коллекция с категориями чая
        /// </summary>
        public readonly static string Categories = "Categories";

        /// <summary>
        /// Коллекция с изрбранным чаем
        /// </summary>
        public readonly static string Favorites = "Favorites";

        /// <summary>
        /// Коллекция с пользователями
        /// </summary>
        public readonly static string Users = "Users";

        /// <summary>
        /// Коллекция с товарами
        /// </summary>
        public readonly static string Products = "Products";

        /// <summary>
        /// Коллекция с купонами
        /// </summary>
        public readonly static string Coupons = "Coupons";

        /// <summary>
        /// Коллекция с рекламой
        /// </summary>
        public readonly static string Promotions = "Promotions";

        /// <summary>
        /// Коллекция с отзывами
        /// </summary>
        public readonly static string Reviews = "Reviews";

        /// <summary>
        /// Коллекция с информацией, какие страницы посещают пользователи
        /// </summary>
        public readonly static string Pages = "Pages";

        /// <summary>
        /// Коллекция с информацией о компаниях
        /// </summary>
        public readonly static string Companies = "Companies";

        /// <summary>
        /// Коллекция с заказами пользователей 
        /// </summary>
        public readonly static string Orders = "Orders";

        /// <summary>
        /// Коллекция с сессиями пользователей
        /// </summary>
        public readonly static string Sessions = "Sessions";

        /// <summary>
        /// Коллекция с действиями пользователей 
        /// </summary>
        public readonly static string UserActions = "UserActions";

        /// <summary>
        /// Коллекция с уведомлениями
        /// </summary>
        public readonly static string Notifications = "Notifications";

        /// <summary>
        /// Коллекция с предсказаниями оттока
        /// </summary>
        public readonly static string ChurnPredictions = "ChurnPredictions";

        /// <summary>
        /// Коллекция с информацией о пользователе
        /// </summary>
        public readonly static string UsersInformation = "UsersInformation";

        /// <summary>
        /// Коллекция с объектами типа MLModelInputs
        /// </summary>
        public readonly static string MLModelInputs = "MLModelInputs";

        /// <summary>
        /// Коллекция с чаем
        /// </summary>
        public readonly static string Teas = "Teas";

        /// <summary>
        /// Коллекция с персоналными завяками пользователей
        /// </summary>
        public readonly static string PersonalUserBids = "PersonalUserBids";

        /// <summary>
        /// Коллекция с счетами к оплате
        /// </summary>
        public readonly static string Invoices = "Invoices";
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

    
