using CustomerChurmPrediction.Entities.UserEntity;
using MongoDB.Driver.Core.Connections;

namespace CustomerChurmPrediction.Services
{
    public interface IConnectionService
    {
        /// <summary>
        /// Добавить подключение
        /// </summary>
        public void AddConnection(string userId, string connectionId);

        /// <summary>
        /// Удалить подключение
        /// </summary>
        public void RemoveConnection(string connectionId);

        /// <summary>
        /// Получить id подключения по id пользователя 
        /// </summary>
        public string? GetConnectionIdByUserId(string userId); 
    }
    public class ConnectionService : IConnectionService
    {
        /// <summary>
        /// Список подключений пользователей
        /// </summary>
        private static readonly Dictionary<string, string> _userConnections = new();

        public void AddConnection(string userId, string connectionId)
        {
            lock (_userConnections)
            {
                _userConnections[userId] = connectionId;
            }
        }

        // может null вернуть
        public string? GetConnectionIdByUserId(string userId)
        {
            lock (_userConnections)
            {
                _userConnections.TryGetValue(userId, out string? connectionId);

                return connectionId;
            }
        }

        public void RemoveConnection(string connectionId)
        {
            lock (_userConnections)
            {
                string userId = _userConnections.FirstOrDefault(kvp => kvp.Value == connectionId).Key;

                if (!string.IsNullOrEmpty(userId))
                    _userConnections.Remove(userId);
            }
        }
    }
}
