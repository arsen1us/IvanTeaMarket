namespace CustomerChurmPrediction.Entities.UserEntity
{
    public class UserReg
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

        public UserReg(string firstName, string lastName, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }
    }
}
