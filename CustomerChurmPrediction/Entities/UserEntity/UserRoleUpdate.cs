namespace CustomerChurmPrediction.Entities.UserEntity
{
    public class UserRoleUpdate
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
    }
}
