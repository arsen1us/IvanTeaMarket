namespace CustomerChurmPrediction.Entities.UserActionEntity
{
    /// <summary>
    /// Действие использования купона
    /// </summary>
    public class UseCouponAction : UserAction
    {
        public string CouponId { get; set; } = null!;
    }
}
