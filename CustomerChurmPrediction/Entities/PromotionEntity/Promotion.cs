namespace CustomerChurmPrediction.Entities.PromotionEntity
{
    // Реклама
    public class Promotion : AbstractEntity
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
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата и время окончания публикации
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Активен пост или нет
        /// </summary>
        public string IsActive {get; set;} = null!;

        /// <summary>
        /// Целевая аудитория
        /// </summary>
        
        public string TargetAudience {get; set;} = null!;

        /// <summary>
        /// Теги
        /// </summary>
        public string Tags {get; set;} = null!;

        /// <summary>
        /// Приоритет 
        /// </summary>
        public string Priority {get; set;} = null!;

        /// <summary>
        /// Место отображения
        /// </summary>
        public string Placement {get; set;} = null!;

        /// <summary>
        /// Количество переходов по ссылке
        /// </summary>
        public string ClickCount {get; set;} = null!;

        /// <summary>
        /// Количество просмотров
        /// </summary>
        public string ViewCount {get; set;} = null!;

        /// <summary>
        /// Показатель кликабельности
        /// </summary>
        public string CTR {get; set;} = null!;

        public Promotion() { }
    }
}
