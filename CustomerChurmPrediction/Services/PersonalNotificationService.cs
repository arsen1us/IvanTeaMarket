namespace CustomerChurmPrediction.Services
{
    public interface IPersonalNotificationService
    {
        /// <summary>
        /// Получить список персональных рекомендаций по продуктам по id пользователей 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<string>> GetPersonalProductListByUserId(string userId, CancellationToken? cancellationToken = default);
    }
    public class PersonalNotificationService : IPersonalNotificationService
    {
        public async Task<List<string>> GetPersonalProductListByUserId(string userId, CancellationToken? cancellationToken = default)
        {
            return new List<string> { };
        }
    }
}
