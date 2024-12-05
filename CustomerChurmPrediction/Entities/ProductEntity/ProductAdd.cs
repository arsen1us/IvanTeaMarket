namespace CustomerChurmPrediction.Entities.ProductEntity
{
	public class ProductAdd : AbstractEntity
	{
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Id категории
        /// </summary>
        public string CategoryId { get; set; } = null!;

        /// <summary>
		/// Id компании
		/// </summary>
		public string CompanyId { get; set; } = null!;

        /// <summary>
		/// Цена
		/// </summary>
		public decimal Price { get; set; }

        /// <summary>
		/// Количество
		/// </summary>
		public int Count { get; set; }

        /// <summary>
		/// Фотографии
		/// </summary>
        public IFormCollection Images { get; set; }
    }
}
