using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Entities;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("api/notify")]
    public class NotificationController(
        ILogger<NotificationController> _logger,
        INotificationService _notificationService,
        IUserConnectionService _userConnectionService,
        IUserService _userService
        ) : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        [Route("{userId}/{type}")]
        public async Task<IActionResult> SendNotificationByUserId(string userId, int type)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("");
                return BadRequest($"Параметр {nameof(userId)} равен null или пуст");
            }
            try
            {
                var existingUser = await _userService.FindByIdAsync(userId, default);

                if(existingUser is null)
                {
                    _logger.LogError("");
                    return NotFound("Не удалось найти пользователя");
                }

                Notification notification = new Notification { UserId = userId };

                throw new Exception();
            }
            catch (Exception ex)
            {
                _logger.LogError("");
                throw new Exception(ex.Message);
            }
        }
    }
}
