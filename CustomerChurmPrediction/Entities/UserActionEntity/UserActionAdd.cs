using CustomerChurmPrediction.Utils;

namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    /// <summary>
    /// Класс для добавления действия пользователя
    /// </summary>
    public class UserActionAdd
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Тип действия пользователя
        /// </summary>
        public UserActionType UserActionType { get; set; }
    }
}
