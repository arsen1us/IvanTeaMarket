namespace CustomerChurmPrediction.Entities.InvoiceEntity
{
    /// <summary>
    /// Счёт к оплате
    /// </summary>
    public class Invoice : AbstractEntity
    {
        /// <summary>
        /// Итоговая цена
        /// </summary>
        public double TotalPrice { get; set; }

        /// <summary>
        /// Список чаёв
        /// </summary>
        public List<InvoiceTea> Teas { get; set; } = null!;

        /// <summary>
        /// Статус
        /// </summary>
        public string Status { get; set; } = null!;
    }

    /// <summary>
    /// Список чаёв и их количества
    /// </summary>
    public class InvoiceTea
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

        /// <summary>
        /// Итоговая цена чая
        /// </summary>
        public double TotalPrice { get; set; }

        public InvoiceTea()
        {
            
        }

        public InvoiceTea(InvoiceTeaAddDto invoiceTeaAdd)
        {
            TeaId = invoiceTeaAdd.TeaId;
            UnitPrice = invoiceTeaAdd.UnitPrice;
            Count = invoiceTeaAdd.Count;
            TotalPrice = UnitPrice * Count;
        }
    }
}
