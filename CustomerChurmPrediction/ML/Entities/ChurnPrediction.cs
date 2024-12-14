using CustomerChurmPrediction.Entities;

namespace CustomerChurmPrediction.ML.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class ChurnPrediction : AbstractEntity
    {
        public bool PredictedChurn { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }
}
