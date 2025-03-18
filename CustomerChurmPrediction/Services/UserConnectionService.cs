using CustomerChurmPrediction.Entities.UserEntity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace CustomerChurmPrediction.Services
{
    public interface IUserConnectionService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connectionId"></param>
        public void AddConnection(string userId, string connectionId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public void RemoveConnection(string userId);

        /// <summary>
        /// Отправить уведомление по id пользователя
        /// </summary>
        /// <param name="method"></param>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendNotificationByUserIdAsync(
            string method,
            string userId,
            string message);

        /// <summary>
        /// Отправить уведомления по id пользователей
        /// </summary>
        /// <param name="method"></param>
        /// <param name="userIds"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendNotificationByUsersIdAsync(
            string method,
            List<string> userIds,
            string message);
    }
    public class UserConnectionService(
        IHubContext<NotificationHub> _hubContext,
        ILogger<UserConnectionService> _logger) : IUserConnectionService
    {
        private static readonly ConcurrentDictionary<string, string> _usersConnection = new ConcurrentDictionary<string, string>();

        public void AddConnection(string userId, string connectionId)
        {
            _usersConnection[userId] = connectionId;
        }

        public void RemoveConnection(string userId)
        {
            _usersConnection.TryRemove(userId, out var _);
        }

        public async Task SendNotificationByUserIdAsync(
            string method,
            string userId,
            string message)
        {
            try
            {
                if (_usersConnection.ContainsKey(userId))
                {
                    string connectionId = _usersConnection[userId];
                    if (!string.IsNullOrEmpty(connectionId))
                    {
                        await _hubContext.Clients.Client(connectionId).SendAsync(method, message);
                    }
                }
                else
                {
                    // Добавить обработку данного случая 
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Произошла ошибка в процессе отправки сообщения по id [{userId}]. Детали ошибки: {ex.Message}");
            }
        }

        public async Task SendNotificationByUsersIdAsync(
            string method,
            List<string> userIds,
            string message)
        {
            for(int i = 0; i < userIds.Count; i++)
            {
                string connectionId = _usersConnection[userIds[i]];
                if (!string.IsNullOrEmpty(connectionId))
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync(method, message);
                }
            }
        }
    }
}
