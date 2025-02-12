using CustomerChurmPrediction;
using CustomerChurmPrediction.Utils;

using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services
    .AddCorsPolicy()
    .AddInfrastructureServices()
    .AddMongoDbService(configuration)
    .AddRabbitMQServices()
    .AddAuthenticationServices(configuration)
    .AddWebApiServices()
    .AddCrudServices()
    .AddUserServices()
    .AddPageServices()
    .AddNotificationServices();

var app = builder.Build();

app.UseStaticFiles();

app.UseCors("default");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapHub<NotificationHub>("/notification-hub");

app.UseSwagger();
app.UseSwaggerUI();

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

// Сохранение на диске или в кэше
var properties = new BasicProperties
{
    Persistent = true
};

while (true)
{
    Console.WriteLine("Enter message:");
    string message = Console.ReadLine();
    var body = Encoding.UTF8.GetBytes(message);

    Console.WriteLine(" Press [enter] to send message");
    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
    Console.WriteLine($" [x] Sent {message}");
    Console.ReadLine();
}

app.Run();
