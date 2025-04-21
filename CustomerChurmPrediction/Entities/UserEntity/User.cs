namespace CustomerChurmPrediction.Entities.UserEntity
{
    public class User : AbstractEntity
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// День рождения
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public string Role { get; set; } = null!;

        /// <summary>
        /// Id компании, с которой может работать пользователь (на основе роли)
        /// </summary>
        public string? CompanyId { get; set; } = null;

        /// <summary>
        /// Фото пользователя
        /// </summary>
        public List<string> ImageSrcs { get; set; } = new List<string>();

        public User(UserReg userReg)
        {
            FirstName = userReg.FirstName;
            LastName = userReg.LastName;
            Email = userReg.Email;
            Password = userReg.Password;
        }
        public User() { }
    }
}
