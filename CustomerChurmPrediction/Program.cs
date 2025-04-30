using CustomerChurmPrediction;
using CustomerChurmPrediction.Utils;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services
    .AddCorsPolicy()
    .AddStrictTransportSecurity()
    .AddInfrastructureServices()
    .AddMongoDbService(configuration)
    .AddAuthenticationServices(configuration)
    .AddWebApiServices()
    .AddCrudServices()
    .AddTelegramBotServices()
    .AddUserServices()
    .AddPageServices()
    .AddNotificationServices()
    .AddBackgroundServices();

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(5000);
//});

var app = builder.Build();

app.UseStaticFiles();

app.UseCors("default");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapHub<NotificationHub>("/notification-hub");

app.UseSwagger();
app.UseSwaggerUI();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy",
        "default-src 'self'; " +
        "script-src 'self' 'unsafe-inline' https://trusted.cdn.com; " +
        "style-src 'self' 'unsafe-inline'; " +
        "img-src 'self' data:; " +
        "connect-src 'self'; " +
        "frame-ancestors 'none';"); // Защита от кликджекинга
    await next();
});

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    await next();
});

app.Use(async (context, next) =>
{
    context.Response.Headers.Remove("X-Powered-By");
    context.Response.Headers.Remove("Server");
    await next();
});

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/login"))
    {
        context.Response.Headers.Add("Cache-Control", "no-store, no-cache");
    }
    await next();
});

app.Run();
