using CustomerChurmPrediction.Entities;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChurmPrediction.Controllers
{
    /// <summary>
    /// Контроллер для работы с логами на клиенте React
    /// </summary>
    [ApiController]
    [Route("api/log")]
    public class LogController(
        ILogger<LogController> _logger,
        ITelegramBotService _telegramBotService) : Controller
    {
        /// <summary>
        /// Добавить лог
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddLog(Log log)
        {
            if (log is null 
                || string.IsNullOrEmpty(log.Type)
                || string.IsNullOrEmpty(log.Message))
            {
                _logger.LogError("");
                await _telegramBotService.SendMessageAsync($"В метод {nameof(AddLog)} не были корректно переданы параметры");
                return BadRequest();
            }

            try
            {
                switch (log.Type)
                {
                    case "info":
                        _logger.LogInformation($"[{DateTime.UtcNow}] [{nameof(AddLog)}] Лог с клиента React.");
                        await _telegramBotService.SendMessageAsync($"[{DateTime.UtcNow}] Лог с клиента React. Type: {log.Type}, message: {log.Message}");
                        return Ok();
                    case "warn":
                        _logger.LogWarning($"[{DateTime.UtcNow}] [{nameof(AddLog)}] Лог с клиента React.");
                        await _telegramBotService.SendMessageAsync($"[{DateTime.UtcNow}] Лог с клиента React. Type: {log.Type}, message: {log.Message}");
                        return Ok();
                    case "error":
                        _logger.LogError($"[{DateTime.UtcNow}] [{nameof(AddLog)}] Лог с клиента React.");
                        await _telegramBotService.SendMessageAsync($"[{DateTime.UtcNow}] Лог с клиента React. Type: {log.Type}, message: {log.Message}");
                        return Ok();
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("");
                await _telegramBotService.SendMessageAsync($"Произошла ошибка в методе {nameof(AddLog)} во время отправки лога");
                throw new Exception(ex.Message);
            }
        }
    }
}
