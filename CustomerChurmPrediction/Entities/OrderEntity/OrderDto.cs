using CustomerChurmPrediction.Entities.TeaEntity;

namespace CustomerChurmPrediction.Entities.OrderEntity
{
    public class OrderDto
    {
        /// <summary>
        /// Заказ
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// Список чая в заказе
        /// </summary>
        public List<Tea> Teas { get; set; }
    }
}
