using CustomerChurmPrediction.ML.Entities;
using CustomerChurmPrediction.Services;
using static CustomerChurmPrediction.Utils.CollectionName;
using MongoDB.Driver;
using CustomerChurmPrediction.Entities.UserEntity;

namespace CustomerChurmPrediction.ML.Services
{
    public interface IChurmPredictionService : IBaseService<ChurnPrediction>
    {
        public void TrainModelByOneUser(UserData userData);
        public void TrainModelByOneUser(List<UserData> userData);
        public ChurnPrediction Predicate(UserData userData);
    }
    public class ChurmPredictionService(IMongoClient client, IConfiguration config, ILogger<UserService> logger, IWebHostEnvironment _environment) 
        : BaseService<ChurnPrediction>(client, config, logger, _environment, ChurnPredictions)
    {

        /// <summary>
        /// Тренировка модели на 1 пользователе
        /// </summary>
        public void TrainModelByOneUser(UserData userData)
        {
            if (userData is null)
                throw new ArgumentNullException(nameof(userData));
            try
            {
                var userDataList = new List<UserData> { userData };

                var churnModel = new ChurnPredictionModel();

                churnModel.TrainModel(userDataList);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Тренировка модели на списке пользователей
        /// </summary>
        public void TrainModelByOneUser(List<UserData> userData)
        {
            if (userData is null)
                throw new ArgumentNullException(nameof(userData));
            try
            {
                var churnModel = new ChurnPredictionModel();

                churnModel.TrainModel(userData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Прогнозирование 
        /// </summary>
        public ChurnPrediction Predicate(UserData userData)
        {
            var churnModel = new ChurnPredictionModel();
            
            ChurnPrediction prediction = churnModel.Predict(userData);

            return prediction;
        }
    }
}
