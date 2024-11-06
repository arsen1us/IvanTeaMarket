namespace CustomerChurmPrediction.Services
{
    public interface IEmailService
    {
        public Task<bool> CheckEmail(string email, CancellationToken? cancellationToken);
    }
    // Сервис для работы с почтой пользователей
    public class EmailService : IEmailService
    {
        // Проверка почты на уникальность
        public async Task<bool> CheckEmail(string email, CancellationToken? cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
