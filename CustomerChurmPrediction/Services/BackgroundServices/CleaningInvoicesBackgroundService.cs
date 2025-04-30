
using CustomerChurmPrediction.Entities.InvoiceEntity;
using CustomerChurmPrediction.Entities.UserEntity;
using MongoDB.Driver;

namespace CustomerChurmPrediction.Services.BackgroundServices
{
    /// <summary>
    /// Фоновый сервис для очистки "просроченных счетов к оплате" (они были созданы, но не оплачены во время или же просто не оплачены)
    /// </summary>
    public class CleaningInvoicesBackgroundService(
        IInvoiceService _invoiceService,
        ILogger<CleaningInvoicesBackgroundService> _logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(ExecuteAsync)}] CleaningInvoicesBackgroundService начал работу");
                while(!stoppingToken.IsCancellationRequested)
                {
                    var invoisecToCancel = await GetInvoicesWithCreatedStatus(stoppingToken);

                    if (!invoisecToCancel.Any())
                    {
                        _logger.LogWarning($"[{DateTime.Now}] Метод [{nameof(ExecuteAsync)}] Полученный список счетов к оплате пуст. Работа сервиса прекращена");
                    }
                    else 
                    {
                        foreach (var invoice in invoisecToCancel)
                        {
                            // Если счёт к оплате создан более 30 минут назад
                            if (invoice.CreateTime <= DateTime.UtcNow - TimeSpan.FromMinutes(30))
                            {
                                invoice.Status = "CanceledByBackgroundService";
                                invoice.LastTimeUserUpdate = DateTime.UtcNow;
                            }
                        }

                        bool isSuccess = await _invoiceService.SaveOrUpdateAsync(invoisecToCancel, stoppingToken);

                        if(isSuccess)
                        {
                            _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(ExecuteAsync)}] Список счетов к оплате успешно обновлён");
                        }
                        else
                        {
                            _logger.LogError($"[{DateTime.Now}] Метод [{nameof(ExecuteAsync)}] Не удалось успешно сохранить обновлённый список счетов к оплате");
                        }
                    }

                    // Выполняется раз в 30 минут
                    await Task.Delay(TimeSpan.FromMinutes(30));
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(ExecuteAsync)}] Во время работы сервиса CleaningInvoicesBackgroundService произошла ошибка. Детали ошибки: {ex.Message}");
                throw new Exception($"[{DateTime.Now}] Метод [{nameof(ExecuteAsync)}] Во время работы сервиса CleaningInvoicesBackgroundService произошла ошибка. Детали ошибки: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<List<Invoice>> GetInvoicesWithCreatedStatus(CancellationToken? cancellationToken = null)
        {
            var filter = Builders<Invoice>.Filter.Eq(invoice => invoice.Status, "Created");
            try
            {
                var invoicesToCancel = await _invoiceService.FindAllAsync(filter, cancellationToken);

                if (invoicesToCancel is null)
                {
                    _logger.LogError($"[{DateTime.Now}] Метод [{nameof(GetInvoicesWithCreatedStatus)}] Не удалось получить список заявок на оплату со статусом Created");
                    throw new Exception();
                }

                _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(GetInvoicesWithCreatedStatus)}] Список заявок на оплату со статусом Created успешно получен. Чсло записей [{invoicesToCancel.Count}]");
                return invoicesToCancel;
            }

            catch(Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(GetInvoicesWithCreatedStatus)}] Во время получения списка заявок на оплату произошла ошибка. Детали ошибки: {ex.Message}");
                throw new Exception($"[{DateTime.Now}] Метод [{nameof(GetInvoicesWithCreatedStatus)}] Во время получения списка заявок на оплату произошла ошибка. Детали ошибки: {ex.Message}");
            }
        }
    }
}
