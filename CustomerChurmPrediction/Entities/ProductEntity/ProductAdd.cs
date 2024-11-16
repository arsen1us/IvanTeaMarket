namespace CustomerChurmPrediction.Entities.ProductEntity
{
	public class ProductAdd : AbstractEntity
	{
		public string Name { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string CategoryId { get; set; } = null!;
		public decimal Price { get; set; }
	}
}
