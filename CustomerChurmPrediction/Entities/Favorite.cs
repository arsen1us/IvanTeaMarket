namespace CustomerChurmPrediction.Entities
{
    public class Favorite : AbstractEntity
    {
        public string ProductId { get; set; } = null!;
        public string UserId { get; set; } = null!;
    }
}
