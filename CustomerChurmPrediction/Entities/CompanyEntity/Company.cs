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
    }
}
