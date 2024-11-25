namespace CustomerChurmPrediction.Entities.PromotionEntity
{
    public class PromotionAdd
    {
        /// <summary>
        /// Название поста
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Контент
        /// </summary>
        public string Content { get; set; } = null!;

        /// <summary>
        /// Ссылка на изображение
        /// </summary>
        public string ImageUrl { get; set; } = null!;

        /// <summary>
        /// Ссылка
        /// </summary>
        public string LinkUrl { get; set; } = null!;

        /// <summary>
        /// Идентификатор компании
        /// </summary>
        public string CompanyId { get; set; } = null!;

        /// <summary>
        /// Дата и время начала публикации
        /// </summary>
        public string StartDate { get; set; } = null!;

        /// <summary>
        /// Дата и время окончания публикации
        /// </summary>
        public string EndDate { get; set; } = null!;

        public PromotionAdd() { }
    }
}
