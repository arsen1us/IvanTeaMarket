namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    /// <summary>
    /// Действие входа пользователя на сайт
    /// </summary>
    public class AuthenticateAction : UserAction
    {
        /// <summary>
        /// Кол-во попыток входа
        /// </summary>
        public int Attempts { get; set; }

        /// <summary>
        /// Успешно ли вошёл в аккаунт
        /// </summary>
        public bool IsSuccess { get; set; }
    }
}
