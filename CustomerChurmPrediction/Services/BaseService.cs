using CustomerChurmPrediction.Entities;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Services
{
    public interface IBaseService<T> where T : AbstractEntity
    {
        // Получить все сущности
        public List<T> FindAll(FilterDefinition<T>? filter);

        // Получить все сущности (async)
        public Task<List<T>> FindAllAsync(FilterDefinition<T>? filter, CancellationToken? cancellationToken = default);

        // Получить сущность по id
        public T FindById(string entityId);

        // Получить сущность по id (async)
        public Task<T> FindByIdAsync(string entityId, CancellationToken? cancellationToken = default);

        // Получить количество сущностей
        public long FindCount(FilterDefinition<T>? filter);

        // Получить количество сущностей (async)
        public Task<long> FindCountAsync(FilterDefinition<T>? filter, CancellationToken? cancellationToken = default);

        // Получить количество сущностей по id сущности
        public long FindCountById(string entityId);

        // Получить количество сущностей по id сущности (async)
        public Task<long> FindCountByIdAsync(string entityId, CancellationToken? cancellationToken = default);

        // Сохранить или обновить сущность
        public bool SaveOrUpdate(T entity);
        public bool SaveOrUpdate(List<T> entities);

        // Сохранить или обновить сущность (async)
        public Task<bool> SaveOrUpdateAsync(T entity, CancellationToken? cancellationToken = default);
        public Task<bool> SaveOrUpdateAsync(List<T> entity, CancellationToken? cancellationToken = default);
        // Удалить сущность по id
        public long Delete(string entityId);

        // Удалить сущность по id (async)
        public Task<long> DeleteAsync(string entityId, CancellationToken? cancellationToken = default);
    }

    public class BaseService<T> : IBaseService<T> where T : AbstractEntity
    {
        IMongoClient _client;
        IConfiguration _config;
        IMongoCollection<T> _collection;
        ILogger _logger;

        public BaseService(IMongoClient client, IConfiguration config, ILogger logger, string collectionName)
        {
            _client = client;
            _config = config;
            _logger = logger;

            var database = _client.GetDatabase(_config["DatabaseConnection:DatabaseName"]);
            _collection = database.GetCollection<T>(collectionName);
        }

        // Получить все сущности
        public virtual List<T> FindAll(FilterDefinition<T>? filter)
        {
            try
            {
                var resultFilter = filter ?? Builders<T>.Filter.Empty;

                var result = _collection.Find(resultFilter);
                return result.ToList();
            }
            catch(Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        // Получить все сущности (async)
        public virtual async Task<List<T>> FindAllAsync(FilterDefinition<T>? filter, CancellationToken? cancellationToken = default)
        {
            try
            {
                var resultFilter = filter ?? Builders<T>.Filter.Empty;
                var result = await _collection.FindAsync(resultFilter);
                return await result.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        // Получить сущность по id
        public virtual T FindById(string entityId)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
                var result = _collection.Find(filter).First();
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        // Получить сущность по id (async)
        public virtual async Task<T> FindByIdAsync(string entityId, CancellationToken? cancellationToken = default)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
                var result = (await _collection.FindAsync(filter)).First();
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        // Получить количество сущностей
        public long FindCount(FilterDefinition<T>? filter)
        {
            var resultFilter = filter ?? Builders<T>.Filter.Empty;
            var result = _collection.CountDocuments(filter);
            return result;
        }

        // Получить количество сущностей (async)
        public async Task<long> FindCountAsync(FilterDefinition<T>? filter, CancellationToken? cancellationToken = default)
        {
            var resultFilter = filter ?? Builders<T>.Filter.Empty;
            var result = await _collection.CountDocumentsAsync(filter);
            return result;
        }

        // Получить количество сущностей по id сущности
        public long FindCountById(string entityId)
        {
            var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
            var result = _collection.CountDocuments(filter);
            return result;
        }

        // Получить количество сущностей по id сущности (async)
        public async Task<long> FindCountByIdAsync(string entityId, CancellationToken? cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
            var result = await _collection.CountDocumentsAsync(filter);
            return result;
        }

        // Сохранить или обновить сущность
        public virtual bool SaveOrUpdate(T entity)
        {
            if(entity != null)
            {
                var entities = new List<T> { entity };
                return SaveOrUpdate(entities);
            }
            return false;
        }

        // Сохранить или обновить список сущностей
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
                    var result = _collection.BulkWrite(bulkOps);

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

        // Сохранить или обновить сущность (async)
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

        // Сохранить или обновить список сущностей (async)
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
                    var result = await _collection.BulkWriteAsync(bulkOps);

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

        // Удалить сущность по id
        public virtual long Delete(string entityId)
        {
            try
            {
                if (entityId != null)
                {
                    var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
                    var result = _collection.DeleteOne(filter);
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

        // Удалить сущность по id (async)
        public virtual async Task<long> DeleteAsync(string entityId, CancellationToken? cancellationToken = default)
        {
            try
            {
                if (entityId != null)
                {
                    var filter = Builders<T>.Filter.Eq(e => e.Id, entityId);
                    var result = await _collection.DeleteOneAsync(filter);
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
    }
}
