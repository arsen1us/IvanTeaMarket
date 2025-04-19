using Telegram.Bot;

namespace CustomerChurmPrediction.Services
{
    public interface ITelegramBotService
    {
        public Task SendMessageAsync(string message, CancellationToken? cancellationToken = null);
    }

    public class TelegramBotService : ITelegramBotService
    {
        private readonly TelegramBotClient _telegramBotClient;
        private readonly long _chatId;
        private ILogger<TelegramBotService> _logger;

        public TelegramBotService(
            IConfiguration config,
            ILogger<TelegramBotService> logger)
        {
            _telegramBotClient = new TelegramBotClient(config["Telegram:Token"]);
            _chatId = long.Parse(config["Telegram:ChatId"]);
            _logger = logger;
        }

        public async Task SendMessageAsync(string message, CancellationToken? cancellationToken = null)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Сообщение не может быть пустым", nameof(message));
            try
            {
                await _telegramBotClient.SendTextMessageAsync(_chatId, message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
