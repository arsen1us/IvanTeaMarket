namespace CustomerChurmPrediction.Entities.PromotionEntity
{
    public class PromotionAdd
    {
        // Название поста.
        public string Title { get; set; } = null!;
        // Текстовое содержимое поста.
        public string Content { get; set; } = null!;
        // Ссылка на изображение (если пост содержит графику).
        public string ImageUrl { get; set; } = null!;
        // Ссылка, куда ведёт реклама(например, на товар или внешнюю страницу).
        public string LinkUrl { get; set; } = null!;
        // Дата и время начала публикации.
        public string StartDate { get; set; } = null!;
        // Дата и время начала публикации.
        public string EndDate { get; set; } = null!;
        // Логическое поле для определения активности поста.
        // public string IsActive { get; set; } = null!;
        // Целевая аудитория (например, демографические данные или категория пользователей).
        // public string TargetAudience { get; set; } = null!;
        // Теги для классификации поста(например, "распродажа", "новинка", "услуги").
        // public string Tags { get; set; } = null!;
        // Идентификатор автора поста(сотрудник или администратор).
        // public string AuthorId { get; set; } = null!;
        // Приоритет отображения(например, для управления сортировкой на сайте).
        // public string Priority { get; set; } = null!;
        // Место отображения(например, "главная страница", "категория товаров", "страница блога").
        // public string Placement { get; set; } = null!;
        // Место отображения(например, "главная страница", "категория товаров", "страница блога").
        // public string ClickCount { get; set; } = null!;
        // Количество просмотров поста.
        // public string ViewCount { get; set; } = null!;
        // Показатель кликабельности(Click-Through Rate) для аналитики.
        // public string CTR { get; set; } = null!;
        // Логическое поле для закрепления поста в верхней части страницы.
        //public string IsPinned { get; set; } = null!;
    }
}
