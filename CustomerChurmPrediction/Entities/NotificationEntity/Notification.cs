namespace CustomerChurmPrediction.Entities.NotificationEntity
{
    public class Notification : AbstractEntity
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Текст уведомления
        /// </summary>
        public string Text { get; set; } = null!;

        /// <summary>
        /// Прочитано ли сообщение
        /// </summary>
        public bool Status { get; set; } = false;
    }
}
