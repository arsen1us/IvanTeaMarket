using CustomerChurmPrediction.Entities;
using CustomerChurmPrediction.Entities.UserEntity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.SignalRMethods;
using static CustomerChurmPrediction.Utils.CollectionName;
using System;

namespace CustomerChurmPrediction.Services
{
    public interface IBaseService<T> where T : AbstractEntity
    {
        /// <summary>
        /// Получить список всех сущностей
        /// </summary>
        public List<T> FindAll(FilterDefinition<T>? filter);

        /// <summary>
        /// Получить список всех сущностей (async)
        /// </summary>
        public Task<List<T>> FindAllAsync(FilterDefinition<T>? filter, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Получить сущность по id
        /// </summary>
        public T FindById(string entityId);

        /// <summary>
        /// Получить сущность по id (async)
        /// </summary>
        public Task<T> FindByIdAsync(string entityId, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Получить список сущностей по id пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<T>> FindByUserIdAsync(string userId, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Получить сущность по id пользователя 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<T> FindOneByUserIdAsync(string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить количество сущностей
        /// </summary>
        public long FindCount(FilterDefinition<T>? filter);

        /// <summary>
        /// Получить количество сущностей (async)
        /// </summary>
        public Task<long> FindCountAsync(FilterDefinition<T>? filter, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Сохранить или обновить сущность
        /// </summary>
        public bool SaveOrUpdate(T entity);

        /// <summary>
        /// Сохранить или обновить список сущностей
        /// </summary>
        public bool SaveOrUpdate(List<T> entities);

        /// <summary>
        /// Сохранить или обновить сущность (async)
        /// </summary>
        public Task<bool> SaveOrUpdateAsync(T entity, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Сохранить или обновить список сущностей (async)
        /// </summary>
        public Task<bool> SaveOrUpdateAsync(List<T> entity, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Удалить сущность по id
        /// </summary>
        public long Delete(string entityId);

        /// <summary>
        /// Удалить сущность по id (async)
        /// </summary>
        public Task<long> DeleteAsync(string entityId, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Сохранить ссылки на изображения
        /// </summary>
        public Task<List<string>> UploadImagesAsync(IFormFileCollection images, CancellationToken? cancellationToken = default);
    }

    /// <summary>
    /// Базовый для всех остальных сервисов сервис
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public class BaseService<T> : IBaseService<T> where T : AbstractEntity
    {
        IMongoClient _client;
        IConfiguration _config;
        ILogger _logger;
        IWebHostEnvironment _environment;
        IHubContext<NotificationHub> _hubContext;
        IUserConnectionService _userConnectionService;

        public IMongoDatabase Database;
        public IMongoCollection<T> Collection;
        public IMongoCollection<User> UserCollection;

        public BaseService(
            IMongoClient client,
            IConfiguration config,
            ILogger logger,
            IWebHostEnvironment environment,
            IHubContext<NotificationHub> hubContext,
            IUserConnectionService userConnectionService,
            string collectionName)
        {
            _client = client;
            _config = config;
            _logger = logger;
            _environment = environment;
            _hubContext = hubContext;
            _userConnectionService = userConnectionService;

            Database = _client.GetDatabase(_config["DatabaseConnection:DatabaseName"]);
            Collection = Database.GetCollection<T>(collectionName);
            UserCollection = Database.GetCollection<User>(Users);
        }

        public virtual List<T> FindAll(FilterDefinition<T>? filter)
        {
            try
            {
                var resultFilter = filter ?? Builders<T>.Filter.Empty;

                var result = Collection.Find(resultFilter);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public virtual async Task<List<T>> FindAllAsync(FilterDefinition<T>? filter = null, CancellationToken? cancellationToken = null)
        {
            try
            {
                var resultFilter = filter ?? Builders<T>.Filter.Empty;
                var cursor = await Collection.FindAsync(resultFilter);
                var result = await cursor.ToListAsync();
                _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(FindAllAsync)}] Успешно получено [{result.Count}] сущностей. Тип сущности: [{typeof(T)}]");
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(FindAllAsync)}] Во время получения всех записей сущности произошла ошибка. Тип сущности: [{typeof(T)}]. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public virtual T FindById(string entityId)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
                var result = Collection.Find(filter).First();
                if (result is not null)
                {
                    _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(FindById)}] Запись с id [{entityId}] успешно получена Тип сущности: [{typeof(T)}]");
                    return result;
                }
                _logger.LogWarning($"[{DateTime.Now}] Метод [{nameof(FindById)}] Не удалось найти запись с id [{entityId}] Тип сущности: [{typeof(T)}]");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(FindById)}] Во время получения записи с id [{entityId}] произошла ошибка. Тип сущности: [{typeof(T)}]. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task<T> FindByIdAsync(string entityId, CancellationToken? cancellationToken = null)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
                var result = (await Collection.FindAsync(filter)).First();
                if (result is not null)
                {
                    _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(FindByIdAsync)}] Запись с id [{entityId}] успешно получена. Тип сущности: [{typeof(T)}]");
                    return result;
                }
                _logger.LogWarning($"[{DateTime.Now}] Метод [{nameof(FindByIdAsync)}] Не удалось найти запись с id [{entityId}]. Тип сущности: [{typeof(T)}]");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(FindByIdAsync)}] Во время получения записи с id [{entityId}] произошла ошибка. Тип сущности: [{typeof(T)}]. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public long FindCount(FilterDefinition<T>? filter)
        {
            try
            {
                var resultFilter = filter ?? Builders<T>.Filter.Empty;
                var result = Collection.CountDocuments(filter);

                _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(FindCount)}] Успешно получено количество записей для коллекции. Кол-во сущностей: [{result}]. Тип сущности: [{typeof(T)}]");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(FindCount)}] Во время получения количества сущностей коллекции произошла ошибка. Тип сущности: [{typeof(T)}]. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<long> FindCountAsync(FilterDefinition<T>? filter, CancellationToken? cancellationToken = null)
        {
            try
            {
                var resultFilter = filter ?? Builders<T>.Filter.Empty;
                var result = await Collection.CountDocumentsAsync(filter);
                _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(FindCount)}] Успешно получено количество записей для коллекции. Кол-во сущностей: [{result}]. Тип сущности: [{typeof(T)}]");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(FindCount)}] Во время получения количества сущностей коллекции произошла ошибка. Тип сущности: [{typeof(T)}]. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public virtual bool SaveOrUpdate(T entity)
        {
            if (entity is null)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(SaveOrUpdate)}] Переданный параметр [{nameof(entity)}] равен null. Тип сущности: [{typeof(T)}]");
                throw new ArgumentNullException();
            }
            try
            {
                var entities = new List<T> { entity };
                bool isSuccess = SaveOrUpdate(entities);
                if (isSuccess)
                {
                    _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(SaveOrUpdate)}] Сущность с id [{entity.Id}] успешно сохранена или обновлена. Тип сущности: [{typeof(T)}]");
                    return isSuccess;
                }
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(SaveOrUpdate)}] Не удалось сохранить или обновить сущность с id [{entity.Id}]. Тип сущности: [{typeof(T)}]");
                return isSuccess;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(SaveOrUpdate)}] Произошла ошибка во время сохранения или обновления сущности с id [{entity.Id}]. Тип сущности: [{typeof(T)}]. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public virtual bool SaveOrUpdate(List<T> entities)
        {
            if (entities is null)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(SaveOrUpdate)}] Переданный параметр [{nameof(entities)}] равен null. Тип сущности: [{typeof(T)}]");
                throw new ArgumentNullException();
            }
            try
            {
                // Создаём список операций ReplaceOneModel для каждого объекта
                var bulkOps = entities
                    .Select(myObject =>
                    {
                        return new ReplaceOneModel<T>(
                            Builders<T>.Filter.Eq(x => x.Id, myObject.Id), // Фильтр по полю Id
                            myObject // Объект для замены
                        )
                        {
                            IsUpsert = true // Если документ не найден, он будет вставлен
                        };
                    })
                    .Cast<WriteModel<T>>() // Приведение к типу WriteModel<T>
                    .ToList();

                // Проверяем, есть ли операции для выполнения
                if (bulkOps.Count > 0)
                {
                    // Выполняем пакетную запись (bulk write) с использованием сессии
                    // var result = await _collection.BulkWriteAsync(session, bulkOps);
                    var result = Collection.BulkWrite(bulkOps);

                    // Проверяем, все ли операции были успешными (либо обновлены, либо вставлены)
                    if (result.Upserts.Count() + result.MatchedCount == entities.Count)
                    {
                        _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(SaveOrUpdate)}] Список сущностей успешно сохранён или обновлён. Количество записей: [{entities.Count}]. Тип сущности: [{typeof(T)}]");
                        return true;
                    }
                }
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(SaveOrUpdate)}] Не удалось сохранить или обновить список сущностей. Количество записей: [{entities.Count}]. Тип сущности: [{typeof(T)}]");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(SaveOrUpdate)}] Произошла ошибка во время сохранения или обновления списка сущностей. Количество записей: [{entities.Count}]. Тип сущности: [{typeof(T)}]. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task<bool> SaveOrUpdateAsync(T entity, CancellationToken? cancellationToken = null)
        {
            if (entity is null)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(SaveOrUpdateAsync)}] Переданный параметр [{nameof(entity)}] равен null. Тип сущности: [{typeof(T)}]");
                throw new ArgumentNullException();
            }

            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            CancellationToken stoppingToken = cancellationTokenSource.Token;

            try
            {
                var entities = new List<T> { entity };
                bool isSuccess = await SaveOrUpdateAsync(entities, stoppingToken);
                if (isSuccess)
                {
                    _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(SaveOrUpdateAsync)}] Сущность с id [{entity.Id}] успешно сохранена или обновлена. Тип сущности: [{typeof(T)}]");
                    return isSuccess;
                }
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(SaveOrUpdateAsync)}] Не удалось сохранить или обновить сущность с id [{entity.Id}]. Тип сущности: [{typeof(T)}]");
                return isSuccess;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(SaveOrUpdateAsync)}] Произошла ошибка во время сохранения или обновления сущности с id [{entity.Id}]. Тип сущности: [{typeof(T)}]. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task<bool> SaveOrUpdateAsync(List<T> entities, CancellationToken? cancellationToken = null)
        {
            if (entities is null)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(SaveOrUpdateAsync)}] Переданный параметр [{nameof(entities)}] равен null. Тип сущности: [{typeof(T)}]");
                throw new ArgumentNullException();
            }

            try
            {
                // Преобразуем переданные объекты в список для удобства обработки
                var abstractEntities = entities.ToList();

                // Создаём список операций ReplaceOneModel для каждого объекта
                var bulkOps = abstractEntities
                    .Select(myObject =>
                    {
                        return new ReplaceOneModel<T>(
                            Builders<T>.Filter.Eq(x => x.Id, myObject.Id), // Фильтр по полю Id
                            myObject // Объект для замены
                        )
                        {
                            IsUpsert = true // Если документ не найден, он будет вставлен
                        };
                    })
                    .Cast<WriteModel<T>>() // Приведение к типу WriteModel<T>
                    .ToList();

                // Проверяем, есть ли операции для выполнения
                if (bulkOps.Count > 0)
                {
                    // Выполняем пакетную запись (bulk write) с использованием сессии
                    // var result = await _collection.BulkWriteAsync(session, bulkOps);
                    var result = await Collection.BulkWriteAsync(bulkOps);

                    // Проверяем, все ли операции были успешными (либо обновлены, либо вставлены)
                    if (result.Upserts.Count() + result.MatchedCount == abstractEntities.Count)
                    {
                        _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(SaveOrUpdateAsync)}] Список сущностей успешно сохранён или обновлён. Количество записей: [{entities.Count}]. Тип сущности: [{typeof(T)}]");
                        return true;
                    }
                }
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(SaveOrUpdateAsync)}] Не удалось сохранить или обновить список сущностей. Количество записей: [{entities.Count}]. Тип сущности: [{typeof(T)}]");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(SaveOrUpdateAsync)}] Произошла ошибка во время сохранения или обновления списка сущностей. Количество записей: [{entities.Count}]. Тип сущности: [{typeof(T)}]. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public virtual long Delete(string entityId)
        {
            if (string.IsNullOrEmpty(entityId))
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(Delete)}] Переданный параметр [{nameof(entityId)}] равен null. Тип сущности: [{typeof(T)}]");
                throw new ArgumentNullException();
            }
            try
            {
                var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
                var result = Collection.DeleteOne(filter);
                if (result.DeletedCount > 0)
                {
                    _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(Delete)}] Сущность с id [{entityId}] успешно удалена. Тип сущности: [{typeof(T)}]");
                    return result.DeletedCount;
                }
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(Delete)}] Не удалось удалить сущность с id [{entityId}]. Тип сущности: [{typeof(T)}]");
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(Delete)}] Во время удаления сущности с id [{entityId}] произошла ошибка. Тип сущности: [{typeof(T)}]. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task<long> DeleteAsync(string entityId, CancellationToken? cancellationToken = null)
        {
            if (string.IsNullOrEmpty(entityId))
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(DeleteAsync)}] Переданный параметр [{nameof(entityId)}] равен null. Тип сущности: [{typeof(T)}]");
                throw new ArgumentNullException();
            }
            try
            {
                var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
                var result = await Collection.DeleteOneAsync(filter);
                if (result.DeletedCount > 0)
                {
                    _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(DeleteAsync)}] Сущность с id [{entityId}] успешно удалена. Тип сущности: [{typeof(T)}]");
                    return result.DeletedCount;
                }

                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(DeleteAsync)}] Не удалось удалить сущность с id [{entityId}]. Тип сущности: [{typeof(T)}]");
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(DeleteAsync)}] Во время удаления сущности с id [{entityId}] произошла ошибка. Тип сущности: [{typeof(T)}]. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<string>> UploadImagesAsync(IFormFileCollection images, CancellationToken? cancellationToken = null)
        {
            if (images is null || images.Count == 0)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(UploadImagesAsync)}] Список изображений пуст. Операция сохранения изображений прекращена");
                // возвращаю пустой список
                return new List<string>();
            }

            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            CancellationToken stoppingToken = cancellationTokenSource.Token;
            var uploadFilePaths = new List<string>();

            try
            {
                // Путь к папке uploads
                var updoadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                foreach (var image in images)
                {

                    var fileExtension = Path.GetExtension(image.FileName).ToLower();

                    // Если у файла другое расширение 
                    if (!allowedExtensions.Contains(fileExtension))
                        // возвращаю пустой список
                        return new List<string>();

                    // Id изображения
                    var imageId = Guid.NewGuid();
                    // Для пользователя
                    var filePath = Path.Combine(imageId + fileExtension);
                    // Для сохранения в файл
                    var folderToSave = Path.Combine(updoadsFolder, imageId + fileExtension);

                    using (var fileStream = new FileStream(folderToSave, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    // Добавление ссылки на фото
                    uploadFilePaths.Add(filePath);
                }
                if (uploadFilePaths is not null)
                {
                    // Если успешно
                    _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(UploadImagesAsync)}] Список изображений успешно сохранён. Количество изображений: [{images.Count}]");
                    return uploadFilePaths;
                }
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(UploadImagesAsync)}] Не удалось сохранить список изображений. Количество изображений: [{images.Count}]");
                // возвращаю пустой список
                return new List<string>();

            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(UploadImagesAsync)}] Во время сохранения списка изображений произошла ошибка. Количество изображений: [{images.Count}]. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<T>> FindByUserIdAsync(string userId, CancellationToken? cancellationToken = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(FindByUserIdAsync)}] Переданный параметр [{nameof(userId)}] равен null. Тип сущности: [{typeof(T)}]");
                throw new ArgumentNullException();
            }
            try
            {
                var userFilter = Builders<User>.Filter.Eq(user => user.Id, userId);
                User existingUser = await (await UserCollection.FindAsync(userFilter)).FirstOrDefaultAsync();

                if (existingUser is null)
                {
                    _logger.LogError($"[{DateTime.Now}] Метод [{nameof(FindByUserIdAsync)}] Не удалось найти пользователя с id [{userId}]. Тип сущности: [{typeof(T)}]");
                    throw new Exception("Не удалось найти пользователя по id. Во время получения сущности по id");
                }

                var filter = Builders<T>.Filter.Eq(entity => entity.UserId, userId);
                List<T> entities = await (await Collection.FindAsync(filter)).ToListAsync();

                _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(FindByUserIdAsync)}] Список записей для пользователь с id [{userId}] успешно получен. Количество записей [{entities.Count}] Тип сущности: [{typeof(T)}]");
                return entities;

            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(FindByUserIdAsync)}] Во время получения списка сущностей для пользователя с id [{userId}] произршла ошибка. Тип сущности: [{typeof(T)}]. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<T> FindOneByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(FindByUserIdAsync)}] Переданный параметр [{userId}] равен null. Тип сущности: [{typeof(T)}]");
                throw new ArgumentNullException();
            }

            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            CancellationToken stoppingToken = cancellationTokenSource.Token;

            try
            {
                var userFilter = Builders<User>.Filter.Eq(user => user.Id, userId);
                User existingUser = await (await UserCollection.FindAsync(userFilter)).FirstOrDefaultAsync(stoppingToken);

                if (existingUser is null)
                {
                    _logger.LogError($"[{DateTime.Now}] Метод [{nameof(FindByUserIdAsync)}] Не удалось найти пользователя с id [{userId}]. Тип сущности: [{typeof(T)}]");
                    throw new Exception("Не удалось найти пользователя по id. Во время получения сущности по id");
                }

                var filter = Builders<T>.Filter.Eq(entity => entity.UserId, userId);
                T entitу = await (await Collection.FindAsync(filter)).FirstOrDefaultAsync(stoppingToken);

                _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(FindByUserIdAsync)}] Запись для пользователя с id [{userId}] успешно получена. Количество записей. Тип сущности: [{typeof(T)}]");
                return entitу;

            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(FindByUserIdAsync)}] Во время получения сущности для пользователя с id [{userId}] произршла ошибка. Тип сущности: [{typeof(T)}]. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
