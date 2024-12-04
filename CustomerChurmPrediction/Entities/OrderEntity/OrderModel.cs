using CustomerChurmPrediction.Entities.ProductEntity;

namespace CustomerChurmPrediction.Entities.OrderEntity
{
    public class OrderModel
    {
        public Order Order { get; set; } = new();
        public Product Product { get; set; } = new();
    }
}
