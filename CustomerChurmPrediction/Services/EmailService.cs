namespace CustomerChurmPrediction.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// Проверка почты на уникальность
        /// </summary>
        public Task<bool> CheckEmail(string email, CancellationToken? cancellationToken);
    }
    public class EmailService : IEmailService
    {
        public async Task<bool> CheckEmail(string email, CancellationToken? cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
