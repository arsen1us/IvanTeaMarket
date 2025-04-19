using CustomerChurmPrediction.Entities.PersonalUserBidEntity;
using CustomerChurmPrediction.Entities.TeaEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("api/personal-order")]
    public class PersonalUserBidController(
        IPersonalUserBidService _personalUserBidService,
        ILogger<PersonalUserBidController> _logger,
        IHubContext<NotificationHub> _hubContext,
        ITelegramBotService _telegramBotService) : Controller
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
                var personalUsersBids = await _personalUserBidService.FindAllAsync(filter, cancellationToken);
                return Ok(new {personalUsersBids =  personalUsersBids});
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

                if (personalUserBid is null)
                    return NotFound();

                return Ok(new { personalUserBid = personalUserBid });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //fixme (надо подумать над этим)
        //[HttpGet]
        //[Route("user/{userId")]
        //public async Task<IActionResult> GetByUserIdAsync(string userId)
        //{
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return BadRequest();
        //    }

        //    using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
        //    CancellationToken cancellationToken = cts.Token;
        //    var filter = Builders<PersonalUserBid>.Filter.Eq(personalUserBid => personalUserBid.UserId)

        //    try
        //    {
                
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] PersonalUserBidAddDto personalUserBidAdd)
        {
            if (personalUserBidAdd is null)
            {
                return BadRequest();
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;

            try
            {
                PersonalUserBid personalUserBid = new PersonalUserBid(personalUserBidAdd);

                bool isSuccess = await _personalUserBidService.SaveOrUpdateAsync(personalUserBid, cancellationToken);

                if (isSuccess)
                {
                    await _telegramBotService.SendMessageAsync($"Новая заявка. Имя - {personalUserBid.Name}, телефон - {personalUserBid.Phone}, почта - {personalUserBid.Email}, детали - {personalUserBid.Details}", cancellationToken);
                    return Ok(new { personalUserBid = personalUserBid });
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="personalUserBidUpdate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] PersonalUserBidUpdateDto personalUserBidUpdate)
        {
            if (string.IsNullOrEmpty(id) || personalUserBidUpdate is null)
            {
                return BadRequest();
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;

            try
            {
                var existingPersonalUserBid = await _personalUserBidService.FindByIdAsync(id, cancellationToken);
                if(existingPersonalUserBid is null)
                {
                    return NotFound();
                }

                if (existingPersonalUserBid.Name != personalUserBidUpdate.Name)
                    existingPersonalUserBid.Name = personalUserBidUpdate.Name;

                if (existingPersonalUserBid.Phone != personalUserBidUpdate.Phone)
                    existingPersonalUserBid.Phone = personalUserBidUpdate.Phone;

                if (existingPersonalUserBid.Email != personalUserBidUpdate.Email)
                    existingPersonalUserBid.Email = personalUserBidUpdate.Email;

                if (existingPersonalUserBid.Details != personalUserBidUpdate.Details)
                    existingPersonalUserBid.Details = personalUserBidUpdate.Details;

                bool isSuccess = await _personalUserBidService.SaveOrUpdateAsync(existingPersonalUserBid, cancellationToken);

                if (isSuccess)
                {
                    return Ok(new { personalUserBid = existingPersonalUserBid });
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id) )
            {
                return BadRequest();
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;

            try
            {
                var deletedCount = await _personalUserBidService.DeleteAsync(id, cancellationToken);

                if(deletedCount == 0)
                {
                    return StatusCode(500);
                }
                else
                {
                    return Ok(new { deletedCount = deletedCount });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
