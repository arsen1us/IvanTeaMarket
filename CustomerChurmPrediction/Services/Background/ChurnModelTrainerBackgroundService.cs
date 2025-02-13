
namespace CustomerChurmPrediction.Services.Background
{
    /// <summary>
    /// Фоновый сервис для получения и отправки данных пользователей для обучения модели
    /// </summary>
    public class ChurnModelTrainerBackgroundService(
        ILogger<ChurnModelTrainerBackgroundService> _logger
        ) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                throw new NotImplementedException();

                while (!stoppingToken.IsCancellationRequested)
                {
                    //var factory = new ConnectionFactory { HostName = "localhost" };
                    //using var connection = await factory.CreateConnectionAsync();
                    //using var channel = await connection.CreateChannelAsync();

                    //await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
                    //    arguments: null);

                    //// Сохранение на диске или в кэше
                    //var properties = new BasicProperties
                    //{
                    //    Persistent = true
                    //};

                    //while (true)
                    //{
                    //    Console.WriteLine("Enter message:");
                    //    string message = Console.ReadLine();
                    //    var body = Encoding.UTF8.GetBytes(message);

                    //    Console.WriteLine(" Press [enter] to send message");
                    //    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
                    //    Console.WriteLine($" [x] Sent {message}");
                    //    Console.ReadLine();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
