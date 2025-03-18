using CustomerChurmPrediction.Entities;
using CustomerChurmPrediction.Entities.UserEntity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.SignalRMethods;
using static CustomerChurmPrediction.Utils.CollectionName;

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

        public virtual async Task<List<T>> FindAllAsync(FilterDefinition<T>? filter = null, CancellationToken? cancellationToken = default)
        {
            try
            {
                var resultFilter = filter ?? Builders<T>.Filter.Empty;
                var result = await Collection.FindAsync(resultFilter);
                return await result.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public virtual T FindById(string entityId)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
                var result = Collection.Find(filter).First();
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public virtual async Task<T> FindByIdAsync(string entityId, CancellationToken? cancellationToken = default)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
                var result = (await Collection.FindAsync(filter)).First();
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public long FindCount(FilterDefinition<T>? filter)
        {
            try
            {
                var resultFilter = filter ?? Builders<T>.Filter.Empty;
                var result = Collection.CountDocuments(filter);
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<long> FindCountAsync(FilterDefinition<T>? filter, CancellationToken? cancellationToken = default)
        {
            try
            {
                var resultFilter = filter ?? Builders<T>.Filter.Empty;
                var result = await Collection.CountDocumentsAsync(filter);
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public virtual bool SaveOrUpdate(T entity)
        {
            if (entity != null)
            {
                var entities = new List<T> { entity };
                return SaveOrUpdate(entities);
            }
            return false;
        }

        public virtual bool SaveOrUpdate(List<T> entities)
        {
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
                    var result = Collection.BulkWrite(bulkOps);

                    // Проверяем, все ли операции были успешными (либо обновлены, либо вставлены)
                    if (result.Upserts.Count() + result.MatchedCount == abstractEntities.Count)
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Логируем исключение и пробрасываем его дальше
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public virtual async Task<bool> SaveOrUpdateAsync(T entity, CancellationToken? cancellationToken = default)
        {
            if (entity is null)
                throw new ArgumentNullException($"Параметр {nameof(entity)} равен null");
            try
            {
                if (entity != null)
                {
                    string userId;

                    if(typeof(T) == typeof(User))
                    {
                        userId = entity.Id;
                    }
                    else
                    {
                        userId = entity.CreatorId;
                    }

                    var entities = new List<T> { entity };
                    bool isSuccess = await SaveOrUpdateAsync(entities, default);
                    if (isSuccess)
                    {
                        await _userConnectionService.SendNotificationByUserIdAsync(SendDatabaseNotification, userId, "Запись успешно сохранена или обновлена");
                        return isSuccess;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public virtual async Task<bool> SaveOrUpdateAsync(List<T> entities, CancellationToken? cancellationToken = default)
        {
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
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public virtual long Delete(string entityId)
        {
            if (string.IsNullOrEmpty(entityId))
                throw new ArgumentNullException(nameof(entityId));
            try
            {
                var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
                var result = Collection.DeleteOne(filter);
                if (result.DeletedCount > 0)
                    return result.DeletedCount;
                return 0;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public virtual async Task<long> DeleteAsync(string entityId, CancellationToken? cancellationToken = default)
        {
            if (string.IsNullOrEmpty(entityId))
                throw new ArgumentNullException(nameof(entityId));
            try
            {
                T entity = await FindByIdAsync(entityId, cancellationToken);

                if (entity is null)
                    throw new Exception($"Не удалось найти запись по id - {entityId}");

                var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
                var result = await Collection.DeleteOneAsync(filter);
                if (result.DeletedCount > 0)
                {
                    // Если данные id разные, отправляю обоим сообщения
                    if (entity.CreatorId != entity.UserIdLastUpdate)
                    {
                        await _userConnectionService.SendNotificationByUsersIdAsync(SendDatabaseNotification, new List<string> { entity.CreatorId, entity.UserIdLastUpdate }, "Запись успешно удалена");
                    }
                    else
                    {
                        await _userConnectionService.SendNotificationByUserIdAsync(SendDatabaseNotification, entity.CreatorId, "Запись успешно удалена");
                    }
                    return result.DeletedCount;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<List<string>> UploadImagesAsync(IFormFileCollection images, CancellationToken? cancellationToken = default)
        {
            if (images is null || images.Count == 0)
                // возвращаю пустой список
                return new List<string>();

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
                // Если успешно
                if (uploadFilePaths is not null)
                    return uploadFilePaths;
                // возвращаю пустой список
                return new List<string>();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<T>> FindByUserIdAsync(string userId, CancellationToken? cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException();
            }
            try
            {
                var userFilter = Builders<User>.Filter.Eq(user => user.Id, userId);
                User existingUser = await (await UserCollection.FindAsync(userFilter)).FirstOrDefaultAsync();

                if (existingUser is null)
                {
                    throw new Exception("Не удалось найти пользователя по id. Во время получения сущности по id");
                }

                var filter = Builders<T>.Filter.Eq(entity => entity.UserId, userId);
                List<T> entities = await (await Collection.FindAsync(filter)).ToListAsync();
                return entities;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T> FindOneByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException();
            }
            try
            {
                var userFilter = Builders<User>.Filter.Eq(user => user.Id, userId);
                User existingUser = await (await UserCollection.FindAsync(userFilter)).FirstOrDefaultAsync();

                if (existingUser is null)
                {
                    throw new Exception("Не удалось найти пользователя по id. Во время получения сущности по id");
                }

                var filter = Builders<T>.Filter.Eq(entity => entity.UserId, userId);
                T entitу = await (await Collection.FindAsync(filter)).FirstOrDefaultAsync();
                return entitу;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
