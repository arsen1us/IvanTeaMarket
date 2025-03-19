using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Entities.SessionEntity;
using CustomerChurmPrediction.Services;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("api/session")]
    public class SessionController(
        ISessionService _sessionService,
        IUserService _userService,
        ILogger<SessionController> _logger) : Controller
    {
        /// <summary>
        /// Создание сессии
        /// </summary>
        // POST: /api/session

        [Authorize(Roles = "User, Admin, Owner")]
        [HttpPost]
        public async Task<IActionResult> CreateSessionAsync([FromBody] SessionAdd sessionAdd)
        {
            if (sessionAdd is null || string.IsNullOrEmpty(sessionAdd.UserId))
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(CreateSessionAsync)}] Переданный параметр [{nameof(sessionAdd)}] равен null");
                return BadRequest();
            }
            try
            {
                using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(1));
                CancellationToken stoppingToken = cancellationTokenSource.Token;

                var user = await _userService.FindByIdAsync(sessionAdd.UserId, stoppingToken);

                var session = new Session
                {
                    UserId = sessionAdd.UserId,
                    SessionTimeStart = DateTime.Now,
                    CreateTime = DateTime.Now,

                    CreatorId = sessionAdd.UserId,
                    UserIdLastUpdate = sessionAdd.UserId,

                };

                bool isSuccess = await _sessionService.SaveOrUpdateAsync(session, stoppingToken);
                if (isSuccess)
                {
                    _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(CreateSessionAsync)}] Сессия [{session.Id}] успешно создана и записана");
                    return Ok();
                }

                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(CreateSessionAsync)}] Не удалось создать и сохранить сессию");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(CreateSessionAsync)}] Во время создания сессии произошла ошибка. Детали ошибки: {ex.Message}");
                throw new Exception();
            }
        }
        /// <summary>
        /// Обновление времени сессии
        /// </summary>
        // PUT: api/session/{userId}

        [Authorize(Roles = "User, Admin, Owner")]
        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> UpdateSessionAsync(string userId, [FromBody] SessionUpdate sessionUpdate)
        {
            if (sessionUpdate is null)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(UpdateSessionAsync)}] Переданный параметр [{nameof(sessionUpdate)}] равен null");
                return BadRequest();
            }
            try
            {
                using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(1));
                CancellationToken stoppingToken = cancellationTokenSource.Token;

                Session lastSession = await _sessionService.GetLastByUserIdAsync(userId, stoppingToken);

                if (lastSession is null) return NotFound();

                lastSession.SessionTimeEnd = DateTime.Now;

                bool isSuccess = await _sessionService.SaveOrUpdateAsync(lastSession, stoppingToken);

                if (isSuccess)
                {
                    _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(UpdateSessionAsync)}] Сессия [{lastSession.Id}] успешно обновлена и записана");
                    return Ok();
                }
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(UpdateSessionAsync)}] Не удалось обновить и сохранить сессию");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(UpdateSessionAsync)}] Во время обновления сессии произошла ошибка. Детали ошибки: {ex.Message}");
                throw new Exception();
            }
        }
    }
}
