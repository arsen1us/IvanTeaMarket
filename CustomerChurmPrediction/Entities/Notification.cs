namespace CustomerChurmPrediction.Entities
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
    }
}
