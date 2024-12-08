namespace CustomerChurmPrediction.Entities.UserEntity
{
    /// <summary>
    /// Взаимодействие с корзиной
    /// </summary>
    public class CartInteraction
    {
        /// <summary>
        /// Количество добавленных в корзину продуктов
        /// </summary>
        public int AddedProductToCartCount { get; set; }

        /// <summary>
        /// CartIds (просто и лаконично)
        /// </summary>
        public List<string> CartIds { get; set; } = new List<string>();

        /// <summary>
        /// Количество удалённых продуктов из корзины 
        /// </summary>
        public int RemovedFromCartCount { get; set; }

        /// <summary>
        /// Процент товаров, которые были сначала добавлены в корзину а потом куплены
        /// </summary>
        // Есть товары, которые были в корзине, но их так и не купили
        public double CartToPurchaseConversionRate { get; set; }
    }
}
