using CustomerChurmPrediction.Entities.InvoiceEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChurmPrediction.Controllers
{
    /// <summary>
    /// Контроллер для обработки HTTP-запросов для работы с счетами к оплате
    /// </summary>
    /// <param name="_userService"></param>
    /// <param name="_telegramBotService"></param>
    /// <param name="_invoiceService"></param>
    /// <param name="_logger"></param>
    [ApiController]
    [Route("/api/invoice")]
    public class InvoiceController(
        IUserService _userService,
        ITelegramBotService _telegramBotService,
        IInvoiceService _invoiceService,
        ITeaService _teaService,
        ILogger<InvoiceController> _logger) : Controller
    {
        /// <summary>
        /// Создаёт счёт на оплату
        /// </summary>
        /// <param name="invoiceAdd"></param>
        /// <returns></returns>
        // POST: /api/invoice

        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(InvoiceAddDto invoiceAdd)
        {
            // Проверка списка чаёв
            if(!invoiceAdd.Teas.Any() || invoiceAdd.Teas == null)
            {
                return BadRequest();
            }

            // Проверка id пользователя
            if(string.IsNullOrEmpty(invoiceAdd.UserId))
            {
                return BadRequest();
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;
            Invoice invoice = new Invoice();
            invoice.Teas = new List<InvoiceTea>(invoiceAdd.Teas.Count);

            try
            {
                // Проверка, существует ли пользователь
                var existingUser = await _userService.FindByIdAsync(invoiceAdd.UserId, cancellationToken);
                if(existingUser is null)
                {
                    return NotFound();
                }


                invoice.UserId = existingUser.Id;
                invoice.UserIdLastUpdate = existingUser.Id;
                invoice.CreateTime = DateTime.UtcNow;
                invoice.Status = "Created";

                
                foreach(var invoiceTeaAdd in invoiceAdd.Teas)
                {
                    var existingTea = await _teaService.FindByIdAsync(invoiceTeaAdd.TeaId, cancellationToken);
                    if(existingTea is null)
                    {
                        return NotFound();
                    }

                    double teaPrice = invoiceTeaAdd.UnitPrice * invoiceTeaAdd.Count;
                    if(teaPrice == 0)
                    {
                        // Добавить обработку данного момента
                    }
                    invoice.TotalPrice += teaPrice;

                    invoice.Teas.Add(new InvoiceTea(invoiceTeaAdd));
                }

                bool isSuccess = await _invoiceService.SaveOrUpdateAsync(invoice, cancellationToken);
                if(!isSuccess)
                {

                    return StatusCode(500);
                }


                return Ok(new { invoice = invoice });
            }
            catch(Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Изменяет счёт на оплату
        /// </summary>
        /// <param name="invoiceUpdate"></param>
        /// <returns></returns>
        // PUT: api/invoice/{id}

        [Authorize(Roles = "User, Admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, InvoiceUpdateDto invoiceUpdate)
        {
            try
            {

                throw new Exception();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        [Authorize(Roles = "User, Admin")]
        [HttpDelete]
        public async Task<IActionResult> CancelAsync(string invoiceId)
        {
            try
            {

                throw new Exception();
            }
            catch(Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
