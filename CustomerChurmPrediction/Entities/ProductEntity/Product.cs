namespace CustomerChurmPrediction.Entities.ProductEntity
{
    public class Product : AbstractEntity
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
		/// Id фотографий
		/// </summary>
        public List<string>? ImageSrcs { get; set; }

        /// <summary>
        /// Информация о скидке
        /// </summary>
        public DiscountInfo Discount { get; set; }
    }
}

