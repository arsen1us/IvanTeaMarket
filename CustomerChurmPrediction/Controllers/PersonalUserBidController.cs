using CustomerChurmPrediction.Entities.PersonalUserBidEntity;
using CustomerChurmPrediction.Entities.TeaEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Controllers
{
    public class PersonalUserBidController(
        IPersonalUserBidService _personalUserBidService,
        ILogger<PersonalUserBidController> _logger,
        IHubContext<NotificationHub> _hubContext) : Controller
    {
        /// <summary>
        /// Получить все записи персональных заявок пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;
            var filter = Builders<PersonalUserBid>.Filter.Empty;

            try
            {
                var personalUserBids = await _personalUserBidService.FindAllAsync(filter, cancellationToken);
                return Ok(new {personalUserBind =  personalUserBids});
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;

            try
            {
                var personalUserBid = await _personalUserBidService.FindByIdAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("user/{userId")]
        public async Task<IActionResult> GetByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] PersonalUserBidAddDto personalUserBidAdd)
        {
            if (personalUserBidAdd is null)
            {
                return BadRequest();
            }

            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> UpdateAsync(string userId, [FromBody] PersonalUserBidUpdateDto personalUserBidUpdate)
        {
            if (string.IsNullOrEmpty(userId) || personalUserBidUpdate is null)
            {
                return BadRequest();
            }

            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<IActionResult> DeleteAsync(string userId)
        {
            if (personalUserBidAdd is null)
            {
                return BadRequest();
            }

            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
