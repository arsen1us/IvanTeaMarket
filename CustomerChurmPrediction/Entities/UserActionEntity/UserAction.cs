using CustomerChurmPrediction.Utils;

namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    public class UserAction : AbstractEntity
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Тип действия пользователя
        /// </summary>
        public UserActionType UserActionType { get; set; }

        /// <summary>
        /// Время действия пользователя
        /// </summary>
        public DateTime ActionDateTime { get; set; }

        /// <summary>
        /// Ссылка на страницу, где было выполнено действие
        /// </summary>
        public string Path { get; set; }

    }
}
