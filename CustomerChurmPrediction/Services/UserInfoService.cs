using CustomerChurmPrediction.Entities;
using CustomerChurmPrediction.Entities.UserEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    public interface IUserInfoService
    {
        /// <summary>
        /// Расчитать взаимодействия пользователя с корзиной
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task CalculateCartInteractionByIdAsync(string userId, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Расссчитать взаимодействия пользователя с заказами
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task CalculateOrderInteractionByIdAsync(string userId, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Расчитать взаимодействия пользователя с рекламой
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task CalculateAdInteractionByIdAsync(string userId, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Расчитать взаимодействия пользователя с купонами
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task CalculateCouponInteractionByIdAsync(string userId, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Расчитать сессии пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task CalculateSessionInfoByIdAsync(string userId, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Расчитать взаимодействия пользователя с отзывами
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task CalculateReviewInteractionByIdAsync(string userId, CancellationToken? cancellationToken = default);
    }

    /// <summary>
    /// Класс для расчёта информации пользователя 
    /// </summary>
    /// <param name="_client">Клиент MongoDB</param>
    /// <param name="_config">Конфигурация</param>
    /// <param name="_logger">Логгер</param>
    /// <param name="_environment">Сведения о среде веб-размещения</param>
    public class UserInfoService(
        IMongoClient _client,
        IConfiguration _config,
        ILogger<UserInfoService> _logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService)
        : BaseService<UserInfo>(_client, _config, _logger, _environment, _hubContext, _userConnectionService, UsersInformation), IUserInfoService
    {
        public async Task CalculateAdInteractionByIdAsync(string userId, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public async Task CalculateCartInteractionByIdAsync(string userId, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public async Task CalculateCouponInteractionByIdAsync(string userId, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public async Task CalculateOrderInteractionByIdAsync(string userId, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public async Task CalculateReviewInteractionByIdAsync(string userId, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public async Task CalculateSessionInfoByIdAsync(string userId, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }
    }
}
