using CustomerChurmPrediction.Entities.UserActionEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("api/action")]
    public class UserActionController(IUserActionService _userActionService) : Controller
    {
        /// <summary>
        /// Добавить действие пользователя
        /// </summary>
        // POST: https://localhost:7299/api/action

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] UserAction userAction)
        {
            if (userAction is null)
                return BadRequest();
            try
            {
                userAction.UserIdLastUpdate = userAction.UserId;
                userAction.ActionTime = DateTime.UtcNow;
                userAction.LastTimeUserUpdate = DateTime.UtcNow;
                userAction.CreatorId = userAction.UserId;

                bool isSuccess = await _userActionService.SaveOrUpdateAsync(userAction, default);
                if (isSuccess)
                    return Ok(new {isSuccess = isSuccess});

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
