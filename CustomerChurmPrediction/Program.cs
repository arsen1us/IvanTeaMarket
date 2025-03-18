using CustomerChurmPrediction;
using CustomerChurmPrediction.Utils;

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
    .AddMLModelServices()
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

app.Run();
