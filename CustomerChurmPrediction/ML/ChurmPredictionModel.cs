using CustomerChurmPrediction.ML.Entities;
using CustomerChurmPrediction.ML.Services;
using Microsoft.ML;

namespace CustomerChurmPrediction.ML
{
    public class ChurnPredictionModel
    {
        private readonly MLContext _mlContext;
        private ITransformer _trainedModel;

        public ChurnPredictionModel()
        {
            _mlContext = new MLContext();
        }

        public void TrainModel(IEnumerable<UserData> trainingData)
        {
            // Загрузка данных
            var data = _mlContext.Data.LoadFromEnumerable(trainingData);

            // Определение шага подготовки данных
            var dataPipeline = _mlContext.Transforms
                .CopyColumns(outputColumnName: "Label", inputColumnName: nameof(UserData.IsLikelyToChurn)) // Указываем метку
                .Append(_mlContext.Transforms.Concatenate("Features", nameof(UserData.TotalOrder),
                                                                     nameof(UserData.TotalPurchases),
                                                                     nameof(UserData.TotalSpent),
                                                                     nameof(UserData.AdClicks),
                                                                     nameof(UserData.LoginFrequency),
                                                                     nameof(UserData.AverageSessionDuration)))
                .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression()); // Убираем MapValueToKey

            // Обучение модели
            _trainedModel = dataPipeline.Fit(data);
        }

        public ChurnPrediction Predict(UserData userData)
        {
            // Создание engine для предсказаний
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<UserData, ChurnPrediction>(_trainedModel);

            // Возвращение результата
            return predictionEngine.Predict(userData);
        }

        public void SaveModel(string modelPath)
        {
            _mlContext.Model.Save(_trainedModel, null, modelPath);
        }

        public void LoadModel(string modelPath)
        {
            _trainedModel = _mlContext.Model.Load(modelPath, out _);
        }
    }
}
