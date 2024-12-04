namespace CustomerChurmPrediction.Entities.UserEntity
{
    public class UserAuth
    {
        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; } = null!;
    }
}
