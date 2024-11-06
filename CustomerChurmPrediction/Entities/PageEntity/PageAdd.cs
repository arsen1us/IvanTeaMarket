namespace CustomerChurmPrediction.Entities.PageEntity
{
    public class PageAdd
    {
        public string UserId { get; set; } = null!;

        // Ссылка на страницу (пока оставлю так, так как нужны проспотры для рекламы, профилей и т.п.)
        public string PageUrl { get; set; } = null!;
    }
}
