namespace CustomerChurmPrediction.Entities.CompanyEntity
{
    public class Company : AbstractEntity
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
        /// Id владельцев компании
        /// </summary>
        public List<string> OwnerIds { get; set; } = new List<string>();

        /// <summary>
		/// Id фотографий
		/// </summary>
        public List<string>? ImageSrcs { get; set; }

        /// <summary>
		/// Электронная почта
		/// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
		/// Номер телефона
		/// </summary>
        public string PhonuNumber { get; set; } = null!;

        /// <summary>
		/// Адрес
		/// </summary>
        public string Address { get; set; } = null!;

        /// <summary>
		/// Регистрационный номер
		/// </summary>
        public string RegistrationNumber { get; set; } = null!;

        /// <summary>
		/// Отрасль
		/// </summary>
        public string Industry { get; set; } = null!;
    }
}
