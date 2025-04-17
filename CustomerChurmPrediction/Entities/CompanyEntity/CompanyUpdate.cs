namespace CustomerChurmPrediction.Entities.CompanyEntity
{
    public class CompanyUpdate
    {
        /// <summary>
        /// Название компании
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Описание компании
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Id пользователя, кто добавляет компанию
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Id владельца компании
        /// </summary>
        public string OwnerId { get; set; } = null!;
    }
}
