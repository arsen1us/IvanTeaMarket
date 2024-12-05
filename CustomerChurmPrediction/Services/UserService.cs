using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;
using CustomerChurmPrediction.Entities.UserEntity;

namespace CustomerChurmPrediction.Services
{
    public interface IUserService : IBaseService<User>
    {
        /// <summary>
        /// Получить пользователя по почте и паролю
        /// </summary>
        public Task<User> FindByEmailAndPassword(string email, string password, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Сохранить ссылки на изображения
        /// </summary>
        public Task<List<string>> UploadImagesAsync(IFormFileCollection images, CancellationToken? cancellationToken = default);
    }

    public class UserService(IMongoClient client, IConfiguration config, ILogger<UserService> logger, IWebHostEnvironment _environment) 
        : BaseService<User>(client, config, logger, Users), IUserService
    {
        public async Task<User> FindByEmailAndPassword(string email, string password, CancellationToken? cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    throw new ArgumentNullException();
                }
                var filter = Builders<User>.Filter.And(
                        Builders<User>.Filter.Eq(u => u.Email, email),
                        Builders<User>.Filter.Eq(u => u.Password, password));

                var user = (await base.FindAllAsync(filter, cancellationToken)).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async  Task<List<string>> UploadImagesAsync(IFormFileCollection images, CancellationToken? cancellationToken = default)
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
