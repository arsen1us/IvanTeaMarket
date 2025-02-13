namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    /// <summary>
    /// Действие регистрации пользователя
    /// </summary>
    public class RegisterAction : UserAction
    {
        /// <summary>
        /// Кол-во попыток регистрации
        /// </summary>
        public int Attempts { get; set; }

        /// <summary>
        /// Успешно ли зарегистрировался
        /// </summary>
        public bool IsSuccess { get; set; }
    }
}
