namespace CustomerChurmPrediction.Entities
{
    public class ChurnPrediction : AbstractEntity
    {
        public bool IsLikelyToChurn { get; set; }

        public float Score { get; set; }
        public float Probability { get; set; }

        public ChurnPrediction(ChurnPredictionDto churnPredictionDto)
        {
            IsLikelyToChurn = churnPredictionDto.IsLikelyToChurn;
            Score = churnPredictionDto.Score;
            Probability = churnPredictionDto.Probability;
        }

        public ChurnPrediction() { }
    }

    public class ChurnPredictionDto
    {
        public bool IsLikelyToChurn { get; set; }

        public float Score { get; set; }
        public float Probability { get; set; }

        public ChurnPredictionDto()
        {

        }
    }
    
}
