namespace CustomerChurmPrediction.Entities.InvoiceEntity
{
    /// <summary>
    /// Класс для создания счета к оплате
    /// </summary>
    public class InvoiceAddDto
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Список чаёв
        /// </summary>
        public List<InvoiceTeaAddDto> Teas { get; set; } = null!;
    }

    /// <summary>
    /// Список чаёв и их количества
    /// </summary>
    public class InvoiceTeaAddDto
    {
        /// <summary>
        /// Id чая
        /// </summary>
        public string TeaId { get; set; } = null!;

        /// <summary>
        /// Количество чая (шт)
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Цена 1 шт на момент выставления счёта
        /// </summary>
        public double UnitPrice { get; set; }
    }
}
