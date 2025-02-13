using CustomerChurmPrediction.Entities;
using MongoDB.Driver;

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
        public IMongoCollection<T> Table;
        ILogger _logger;
        IWebHostEnvironment _environment;

        public IMongoDatabase Database;

        public BaseService(
            IMongoClient client,
            IConfiguration config,
            ILogger logger,
            IWebHostEnvironment environment,
            string collectionName)
        {
            _client = client;
            _config = config;
            _logger = logger;
            _environment = environment;

            Database = _client.GetDatabase(_config["DatabaseConnection:DatabaseName"]);
            Table = Database.GetCollection<T>(collectionName);
        }

        public virtual List<T> FindAll(FilterDefinition<T>? filter)
        {
            try
            {
                var resultFilter = filter ?? Builders<T>.Filter.Empty;

                var result = Table.Find(resultFilter);
                return result.ToList();
            }
            catch(Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public virtual async Task<List<T>> FindAllAsync(FilterDefinition<T>? filter = null, CancellationToken? cancellationToken = default)
        {
            try
            {
                var resultFilter = filter ?? Builders<T>.Filter.Empty;
                var result = await Table.FindAsync(resultFilter);
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
                var result = Table.Find(filter).First();
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
                var result = (await Table.FindAsync(filter)).First();
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
                var result = Table.CountDocuments(filter);
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
                var result = await Table.CountDocumentsAsync(filter);
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public virtual bool SaveOrUpdate(T entity)
        {
            if(entity != null)
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
                    var result = Table.BulkWrite(bulkOps);

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
            try
            {
                if(entity != null)
                {
                    var entities = new List<T> { entity };
                    return await SaveOrUpdateAsync(entities, default);
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
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
                    var result = await Table.BulkWriteAsync(bulkOps);

                    // Проверяем, все ли операции были успешными (либо обновлены, либо вставлены)
                    if (result.Upserts.Count() + result.MatchedCount == abstractEntities.Count)
                        return true;
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
            try
            {
                if (entityId != null)
                {
                    var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
                    var result = Table.DeleteOne(filter);
                    if (result.DeletedCount > 0)
                        return result.DeletedCount;
                    return 0;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public virtual async Task<long> DeleteAsync(string entityId, CancellationToken? cancellationToken = default)
        {
            try
            {
                if (entityId != null)
                {
                    var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
                    var result = await Table.DeleteOneAsync(filter);
                    if (result.DeletedCount > 0)
                        return result.DeletedCount;
                    return 0;
                }
                else
                {
                    throw new ArgumentNullException();
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
    }
}
