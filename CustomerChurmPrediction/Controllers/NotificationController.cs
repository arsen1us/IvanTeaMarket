using CustomerChurmPrediction.Entities.NotificationEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("api/notification")]
    public class NotificationController : Controller
    {
        IUserService _userService;
        INotificationService _notificationService;

        public NotificationController(IUserService userService, INotificationService notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) 
                return BadRequest(); 
            try
            {
                var user = await _userService.FindByIdAsync(userId, default);
                if(user is null)
                    return NotFound();

                var notificationList = await _notificationService.FindByUserIdAsync(userId, default);
                if(notificationList is not null)
                    return Ok(new {notificationList = notificationList});

                return NotFound();
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
