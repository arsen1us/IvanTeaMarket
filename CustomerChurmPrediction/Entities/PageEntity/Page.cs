namespace CustomerChurmPrediction.Entities.PageEntity
{
    // Вкладка сайта (страница с товарами)
    public class Page : AbstractEntity
    {
        public string UserId { get; set; } = null!;

        // Ссылка на страницу (пока оставлю так, так как нужны проспотры для рекламы, профилей и т.п.)
        public string PageUrl { get; set; } = null!;
    }
}
