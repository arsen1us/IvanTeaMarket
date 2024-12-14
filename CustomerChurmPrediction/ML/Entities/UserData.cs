using CustomerChurmPrediction.Entities;

namespace CustomerChurmPrediction.ML.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class UserData : AbstractEntity
    {
        public float TotalOrder { get; set; }
        public float TotalPurchases { get; set; }
        public float TotalSpent { get; set; }
        public float AdClicks { get; set; }
        public float LoginFrequency { get; set; }
        public float AverageSessionDuration { get; set; } // в секундах
        public bool IsLikelyToChurn { get; set; } // Метка (цель)
    }
}
