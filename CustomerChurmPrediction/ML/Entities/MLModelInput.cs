using CustomerChurmPrediction.Entities;

namespace CustomerChurmPrediction.ML.Entities
{
    /// <summary>
    /// Данные, которые подаются на вход моделм машинного обучения 
    /// </summary>
    public class MLModelInput : AbstractEntity
    {
        /// <summary>
        /// Метка оттока (true - пользователь ушёл, false - остался)
        /// </summary>
        public bool IsLikelyToChurn { get; set; }

        /// <summary>
        /// Общее количество добавлений в корзину
        /// </summary>
        public int TotalCartAdds { get; set; }

        /// <summary>
        /// Среднее время между добавлениями (минуты)
        /// </summary>
        public double AvgTimeBetweenAdds { get; set; }

        /// <summary>
        /// Максимальное время между добавлениями (минуты)
        /// </summary>
        public double MaxTimeBetweenAdds { get; set; }

        /// <summary>
        /// Время с последнего добавления
        /// </summary>
        public double TimeSinceLastAdd { get; set; }

        /// <summary>
        /// Уникальные товары в корзине
        /// </summary>
        public int CartUniqueProducts { get; set; }

        /// <summary>
        /// Общее число заказов
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// Средняя стоимость заказа
        /// </summary>
        public double AvgOrderPrice { get; set; }

        /// <summary>
        /// Максимальная стоимость заказа
        /// </summary>
        public double MaxOrderPrice { get; set; }

        /// <summary>
        /// Среднее число позиций в заказе
        /// </summary>
        public double AvgItemsPerOrder { get; set; }

        /// <summary>
        /// Количество уникальных товаров
        /// </summary>
        public int OrderUniqueProducts { get; set; }

        /// <summary>
        /// Время с последнего заказа (в минутах)
        /// </summary>
        public double MinutesSinceLastOrder { get; set; }

        /// <summary>
        /// Средний интервал между заказами (в минутах)
        /// </summary>
        public double AvgMinutesBetweenOrders { get; set; }

        /// <summary>
        /// Количество отменённых заказов
        /// </summary>
        public int CancelledOrders { get; set; }

        /// <summary>
        /// Общее количество посещений страниц
        /// </summary>
        public int TotalPageViews { get; set; }

        /// <summary>
        /// Количество уникальных страниц
        /// </summary>
        public int UniquePages { get; set; }

        /// <summary>
        /// Среднее время между просмотрами страниц (в минутах)
        /// </summary>
        public double AvgTimeBetweenViews { get; set; }

        /// <summary>
        /// Максимальное время между просмотрами страниц (в минутах)
        /// </summary>
        public double MaxTimeBetweenViews { get; set; }

        /// <summary>
        /// Время с последнего просмотра (в минутах)
        /// </summary>
        public double TimeSinceLastView { get; set; }

        /// <summary>
        /// Общее количество сессий
        /// </summary>
        public int TotalSessions { get; set; }

        /// <summary>
        /// Средняя длительность сессии (в минутах)
        /// </summary>
        public double AvgSessionDuration { get; set; }

        /// <summary>
        /// Максимальная длительность сессии (в минутах)
        /// </summary>
        public double MaxSessionDuration { get; set; }

        /// <summary>
        /// Средний интервал между сессиями (в минутах)
        /// </summary>
        public double AvgTimeBetweenSessions { get; set; }

        /// <summary>
        /// Время с последнего визита (в минутах)
        /// </summary>
        public double TimeSinceLastSession { get; set; }

        /// <summary>
        /// Общее количество действий
        /// </summary>
        public int TotalActions { get; set; }

        /// <summary>
        /// Количество действий по типу
        /// </summary>
        // public Dictionary<string, int> ActionsByType { get; set; } = new();

        /// <summary>
        /// Средний интервал между действиями (в минутах)
        /// </summary>
        public double AvgTimeBetweenActions { get; set; }

        /// <summary>
        /// Максимальный интервал между действиями (в минутах)
        /// </summary>
        public double MaxTimeBetweenActions { get; set; }

        /// <summary>
        /// Время с последнего действия (в минутах)
        /// </summary>
        public double TimeSinceLastAction { get; set; }

        public MLModelInput(
            CartInteraction cartInteraction,
            OrderInteraction orderInteraction,
            PageInteraction pageInteractions,
            SessionInteraction sessionInteractions,
            UserActionStat userActionStats)
        {
            TotalCartAdds = cartInteraction.TotalCartAdds;
            AvgTimeBetweenAdds = cartInteraction.AvgTimeBetweenAdds;
            MaxTimeBetweenAdds = cartInteraction.MaxTimeBetweenAdds;
            TimeSinceLastAdd = cartInteraction.TimeSinceLastAdd;
            CartUniqueProducts = cartInteraction.UniqueProducts;

            TotalOrders = orderInteraction.TotalOrders;
            AvgOrderPrice = orderInteraction.AvgOrderPrice;
            MaxOrderPrice = orderInteraction.MaxOrderPrice;
            AvgItemsPerOrder = orderInteraction.AvgItemsPerOrder;
            OrderUniqueProducts = orderInteraction.UniqueProducts;
            MinutesSinceLastOrder = orderInteraction.MinutesSinceLastOrder;
            AvgMinutesBetweenOrders = orderInteraction.AvgMinutesBetweenOrders;
            CancelledOrders = orderInteraction.CancelledOrders;

            TotalPageViews = pageInteractions.TotalPageViews;
            UniquePages = pageInteractions.UniquePages;
            AvgTimeBetweenViews = pageInteractions.AvgTimeBetweenViews;
            MaxTimeBetweenViews = pageInteractions.MaxTimeBetweenViews;
            TimeSinceLastView = pageInteractions.TimeSinceLastView;

            TotalSessions = sessionInteractions.TotalSessions;
            AvgSessionDuration = sessionInteractions.AvgSessionDuration;
            MaxSessionDuration = sessionInteractions.MaxSessionDuration;
            AvgTimeBetweenSessions = sessionInteractions.AvgTimeBetweenSessions;
            TimeSinceLastSession = sessionInteractions.TimeSinceLastSession;

            TotalActions = userActionStats.TotalActions;
            // ActionsByType = userActionStats.ActionsByType;
            AvgTimeBetweenActions = userActionStats.AvgTimeBetweenActions;
            MaxTimeBetweenActions = userActionStats.MaxTimeBetweenActions;
            TimeSinceLastAction = userActionStats.TimeSinceLastAction;
        }

        /// <summary>
        /// Скопировать все расчитанные поля
        /// </summary>
        /// <param name="cartInteraction"></param>
        /// <param name="orderInteraction"></param>
        /// <param name="pageInteractions"></param>
        /// <param name="sessionInteractions"></param>
        /// <param name="userActionStats"></param>
        public void CopyCalculatedFields(
            CartInteraction cartInteraction,
            OrderInteraction orderInteraction,
            PageInteraction pageInteractions,
            SessionInteraction sessionInteractions,
            UserActionStat userActionStats)
        {
            TotalCartAdds = cartInteraction.TotalCartAdds;
            AvgTimeBetweenAdds = cartInteraction.AvgTimeBetweenAdds;
            MaxTimeBetweenAdds = cartInteraction.MaxTimeBetweenAdds;
            TimeSinceLastAdd = cartInteraction.TimeSinceLastAdd;
            CartUniqueProducts = cartInteraction.UniqueProducts;

            TotalOrders = orderInteraction.TotalOrders;
            AvgOrderPrice = orderInteraction.AvgOrderPrice;
            MaxOrderPrice = orderInteraction.MaxOrderPrice;
            AvgItemsPerOrder = orderInteraction.AvgItemsPerOrder;
            OrderUniqueProducts = orderInteraction.UniqueProducts;
            MinutesSinceLastOrder = orderInteraction.MinutesSinceLastOrder;
            AvgMinutesBetweenOrders = orderInteraction.AvgMinutesBetweenOrders;
            CancelledOrders = orderInteraction.CancelledOrders;

            TotalPageViews = pageInteractions.TotalPageViews;
            UniquePages = pageInteractions.UniquePages;
            AvgTimeBetweenViews = pageInteractions.AvgTimeBetweenViews;
            MaxTimeBetweenViews = pageInteractions.MaxTimeBetweenViews;
            TimeSinceLastView = pageInteractions.TimeSinceLastView;

            TotalSessions = sessionInteractions.TotalSessions;
            AvgSessionDuration = sessionInteractions.AvgSessionDuration;
            MaxSessionDuration = sessionInteractions.MaxSessionDuration;
            AvgTimeBetweenSessions = sessionInteractions.AvgTimeBetweenSessions;
            TimeSinceLastSession = sessionInteractions.TimeSinceLastSession;

            TotalActions = userActionStats.TotalActions;
            // ActionsByType = userActionStats.ActionsByType;
            AvgTimeBetweenActions = userActionStats.AvgTimeBetweenActions;
            MaxTimeBetweenActions = userActionStats.MaxTimeBetweenActions;
            TimeSinceLastAction = userActionStats.TimeSinceLastAction;
        }
    }

    public class MLModelInputDto
    {
        public bool IsLikelyToChurn { get; set; }
        public float TotalCartAdds { get; set; }
        public float AvgTimeBetweenAdds { get; set; }
        public float MaxTimeBetweenAdds { get; set; }
        public float TimeSinceLastAdd { get; set; }
        public float CartUniqueProducts { get; set; }
        public float TotalOrders { get; set; }
        public float AvgOrderPrice { get; set; }
        public float MaxOrderPrice { get; set; }
        public float AvgItemsPerOrder { get; set; }
        public float OrderUniqueProducts { get; set; }
        public float MinutesSinceLastOrder { get; set; }
        public float AvgMinutesBetweenOrders { get; set; }
        public float CancelledOrders { get; set; }
        public float TotalPageViews { get; set; }
        public float UniquePages { get; set; }
        public float AvgTimeBetweenViews { get; set; }
        public float MaxTimeBetweenViews { get; set; }
        public float TimeSinceLastView { get; set; }
        public float TotalSessions { get; set; }
        public float AvgSessionDuration { get; set; }
        public float MaxSessionDuration { get; set; }
        public float AvgTimeBetweenSessions { get; set; }
        public float TimeSinceLastSession { get; set; }
        public float TotalActions { get; set; }
        public float AvgTimeBetweenActions { get; set; }
        public float MaxTimeBetweenActions { get; set; }
        public float TimeSinceLastAction { get; set; }

        public MLModelInputDto(MLModelInput entity)
        {
            IsLikelyToChurn = entity.IsLikelyToChurn;
            TotalCartAdds = entity.TotalCartAdds;
            AvgTimeBetweenAdds = (float)entity.AvgTimeBetweenAdds;
            MaxTimeBetweenAdds = (float)entity.MaxTimeBetweenAdds;
            TimeSinceLastAdd = (float)entity.TimeSinceLastAdd;
            CartUniqueProducts = entity.CartUniqueProducts;
            TotalOrders = entity.TotalOrders;
            AvgOrderPrice = (float)entity.AvgOrderPrice;
            MaxOrderPrice = (float)entity.MaxOrderPrice;
            AvgItemsPerOrder = (float)entity.AvgItemsPerOrder;
            OrderUniqueProducts = entity.OrderUniqueProducts;
            MinutesSinceLastOrder = (float)entity.MinutesSinceLastOrder;
            AvgMinutesBetweenOrders = (float)entity.AvgMinutesBetweenOrders;
            CancelledOrders = entity.CancelledOrders;
            TotalPageViews = entity.TotalPageViews;
            UniquePages = entity.UniquePages;
            AvgTimeBetweenViews = (float)entity.AvgTimeBetweenViews;
            MaxTimeBetweenViews = (float)entity.MaxTimeBetweenViews;
            TimeSinceLastView = (float)entity.TimeSinceLastView;
            TotalSessions = entity.TotalSessions;
            AvgSessionDuration = (float)entity.AvgSessionDuration;
            MaxSessionDuration = (float)entity.MaxSessionDuration;
            AvgTimeBetweenSessions = (float)entity.AvgTimeBetweenSessions;
            TimeSinceLastSession = (float)entity.TimeSinceLastSession;
            TotalActions = entity.TotalActions;
            AvgTimeBetweenActions = (float)entity.AvgTimeBetweenActions;
            MaxTimeBetweenActions = (float)entity.MaxTimeBetweenActions;
            TimeSinceLastAction = (float)entity.TimeSinceLastAction;
        }
    }
}
