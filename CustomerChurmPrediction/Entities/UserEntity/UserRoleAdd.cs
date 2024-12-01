namespace CustomerChurmPrediction.Entities.UserEntity
{
    /// <summary>
    /// Класс для добавления роли пользователю
    /// </summary>
    public class UserRoleAdd
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public string Role { get; set; } = null!;
        
        /// <summary>
        /// Id компании
        /// </summary>
        public string CompanyId { get; set; } = null!;
        public UserRoleAdd() { }
    }
}
