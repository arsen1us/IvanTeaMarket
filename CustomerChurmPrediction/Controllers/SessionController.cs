using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Entities.SessionEntity;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("api/session")]
    public class SessionController : Controller
    {
        /// <summary>
        /// Создание сессии
        /// </summary>
        // POST: /api/session

        [Authorize(Roles = "User, Admin, Owner")]
        [HttpPost]
        public async Task<IActionResult> CreateSessionAsync(SessionAdd sessionAdd)
        {
            if (sessionAdd is null)
                return BadRequest();
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}
