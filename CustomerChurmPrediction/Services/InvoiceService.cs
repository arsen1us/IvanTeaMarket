using CustomerChurmPrediction.Entities.InvoiceEntity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using static CustomerChurmPrediction.Utils.CollectionName;

namespace CustomerChurmPrediction.Services
{
    /// <summary>
    /// Сервис для работы с счетами к оплате
    /// </summary>
    public interface IInvoiceService : IBaseService<Invoice>
    {

    }

    public class InvoiceService(
        IMongoClient client,
        IConfiguration config,
        ILogger<CartService> logger,
        IWebHostEnvironment _environment,
        IHubContext<NotificationHub> _hubContext,
        IUserConnectionService _userConnectionService) : BaseService<Invoice>(client, config, logger, _environment, _hubContext, _userConnectionService, Invoices), IInvoiceService
    {

    }
}
