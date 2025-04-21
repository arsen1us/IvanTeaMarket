using CustomerChurmPrediction.Entities.TeaEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("api/tea")]
    public class TeaController(
        ITeaService _teaService,
        ILogger<TeaController> _logger,
        IHubContext<NotificationHub> _hubContext) : Controller
    {
        /// <summary>
        /// Получить список всех чаёв
        /// </summary>
        // GET: https://localhost:7299/api/tea

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;
            var filter = Builders<Tea>.Filter.Empty;
            try
            {
                List<Tea> teas = await _teaService.FindAllAsync(filter, cancellationToken);
                _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(GetAllAsync)}] - Успешно получен список чаёв. Число записей: {teas.Count}");
                return Ok(new { teas = teas });
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetAllAsync)}] - Не удалось получить список чаёв. Детали ошибки: {ex.Message}");
                throw new Exception($"[{DateTime.UtcNow} Method: {nameof(GetAllAsync)}] - Не удалось получить список чаёв. Детали ошибки: {ex.Message}");
            }
        }

        /// <summary>
        /// Получить чай по id
        /// </summary>
        // GET: https://localhost:7299/api/tea/{teaId}

        [HttpGet]
        [Route("{teaId}")]
        public async Task<IActionResult> GetByIdAsync(string teaId)
        {
            if (string.IsNullOrEmpty(teaId))
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByIdAsync)}] - Параметр {nameof(teaId)} не был передан");
                return BadRequest();
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;
            try
            {
                Tea tea = await _teaService.FindByIdAsync(teaId, cancellationToken);
                if (tea is null)
                {
                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByIdAsync)}] - Не удалось найти запись с id {teaId}");
                    return NotFound();

                }

                _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(GetByIdAsync)}] - Запись с id {teaId} успешно получена");
                return Ok(new { tea = tea });

            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(GetByIdAsync)}] - Не удалосб получить запись с id {teaId}. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Добавить новую запись с чаем
        /// </summary>
        // POST: https://localhost:7299/api/tea

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromForm] TeaAddDto teaAddDto)
        {
            if (teaAddDto is null)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddAsync)}] - Параметр {nameof(teaAddDto)} не был передан");
                return BadRequest();
            }

            // Получение и запись изображений
            if (teaAddDto.Images is null || teaAddDto.Images.Count == 0)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddAsync)}] - Обязательно необходимо передать список изображений");
                return BadRequest();
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;

            try
            {
                Tea tea = new Tea(teaAddDto)
                {
                    CreateTime = DateTime.Now,
                    LastTimeUserUpdate = DateTime.Now,
                };

                // Загрузка изображений на сервер
                List<string> imageSrcs = await _teaService.UploadImagesAsync(teaAddDto.Images, cancellationToken);
                if (imageSrcs is null)
                {
                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddAsync)}] - Не удалось сохранить изображения для продукта");
                    return StatusCode(500);
                }
                tea.ImageSrcs = imageSrcs;

                bool isSuccess = await _teaService.SaveOrUpdateAsync(tea, cancellationToken);
                if (isSuccess)
                {
                    _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(AddAsync)}] - Чай успешно добавлен");
                    await _hubContext.Clients.All.SendAsync("ReceiveNotification", "Чай успешно добавлен!", cancellationToken);
                    return Ok(new { tea = tea });
                }

                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddAsync)}] - Не удалось сохранить чай");
                return StatusCode(500);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(AddAsync)}] - Произошла ошибка во время добавления чая. Детали ошибки: {ex.Message}");
                throw new Exception($"Произошла ошибка во время добавления чая. Детали ошибки: {ex.Message}");
            }
        }
        /// <summary>
        /// Изменить запись с чаем по id 
        /// </summary>
        // PUT: https://localhost:7299/api/tea/{teaId}

        [HttpPut]
        [Route("{teaId}")]
        public async Task<IActionResult> UpdateAsync(string teaId, [FromBody] TeaUpdateDto teaUpdateDto)
        {
            if (teaId is null || teaUpdateDto is null)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(UpdateAsync)}] - Параметры {nameof(teaId)}, {nameof(teaUpdateDto)} не был передан");
                return BadRequest();
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;

            try
            {
                var existingTea = await _teaService.FindByIdAsync(teaId, cancellationToken);
                if (existingTea != null)
                {
                    // Обновление названия, если оно обновлено
                    if (existingTea.Name == teaUpdateDto.Name)
                        existingTea.Name = teaUpdateDto.Name;
                    // Обновление цены, если оно обновлено 
                    if (existingTea.Price == teaUpdateDto.Price)
                        existingTea.Price = teaUpdateDto.Price;
                    // Обновление типа упаковки, если оно обновлено 
                    if (existingTea.PackageType == teaUpdateDto.PackageType)
                        existingTea.PackageType = teaUpdateDto.PackageType;
                    // Обновление материала упаковки, если оно обновлено 
                    if (existingTea.PackageMaterials == teaUpdateDto.PackageMaterials)
                        existingTea.PackageMaterials = teaUpdateDto.PackageMaterials;
                    // Обновление веса продукта, если он обновлён 
                    if (existingTea.Weight == teaUpdateDto.Weight)
                        existingTea.Weight = teaUpdateDto.Weight;
                    // Обновление доп. информации о весе, если она обновлена 
                    if (existingTea.WeightDetails == teaUpdateDto.WeightDetails)
                        existingTea.WeightDetails = teaUpdateDto.WeightDetails;

                    var isSuccess = await _teaService.SaveOrUpdateAsync(existingTea, cancellationToken);
                    if (isSuccess)
                    {
                        _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(UpdateAsync)}] - Чай с id {teaId} успешно обновлён");
                        return Ok(new { tea = existingTea });
                    }

                    _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(UpdateAsync)}] - Не удалось обновить чай с id {nameof(teaId)}");
                    return StatusCode(500);
                }
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(UpdateAsync)}] - Не удалось найти чай с id {nameof(teaId)}");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(UpdateAsync)}] - Произошла ошибка при обновлении чая с id {nameof(teaId)}. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Удалить запись с чаем по id 
        /// </summary>
        // Delete: https://localhost:7299/api/tea/{teaId}

        [HttpDelete]
        [Route("{teaId}")]
        public async Task<IActionResult> DeleteAsync(string teaId)
        {
            if (string.IsNullOrEmpty(teaId))
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(DeleteAsync)}] - Параметр {nameof(teaId)} не был передан");
                return BadRequest();
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;
            try
            {
                var deletedCount = await _teaService.DeleteAsync(teaId, cancellationToken);
                if (deletedCount > 0)
                {
                    _logger.LogInformation($"[{DateTime.UtcNow} Method: {nameof(DeleteAsync)}] - Продукт с id {teaId} успешно удалён. Число удалённых записей: {deletedCount}");
                    return Ok(new { deletedCount = deletedCount });
                }

                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(DeleteAsync)}] - Не удалось найти продукт с id {nameof(teaId)}");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.UtcNow} Method: {nameof(UpdateAsync)}] - Произошла ошибка при удалении продукта с id {nameof(teaId)}. Детали ошибки: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
