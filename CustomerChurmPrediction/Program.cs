using MongoDB.Driver;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CustomerChurmPrediction;
using CustomerChurmPrediction.ML.Entities;
using CustomerChurmPrediction.ML;
using CustomerChurmPrediction.Entities.NotificationEntity;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
options.AddPolicy("default", policy =>
{
    policy.WithOrigins("http://localhost:3000") // React
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials(); // для SignalR
}));

var configuration = builder.Configuration;

// Подключение к базе данных
builder.Services.AddSingleton<IMongoClient, MongoClient>(options =>
{
    return new MongoClient(configuration["DatabaseConnection:ConnectionString"]);
});


builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = configuration["TokenSettings:Issuer"],
        ValidAudience = configuration["TokenSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenSettings:Key"]))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(accessToken) &&
                context.HttpContext.Request.Path.StartsWithSegments("/notification-hub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// Подключение сервисов для взаимодействия с бд
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserActionService, UserActionService>();
builder.Services.AddScoped<IUserService, UserService>();

// Сервис для работы с подключениями пользователей
builder.Services.AddScoped<IConnectionService, ConnectionService>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

// Подключение логирования
builder.Services.AddLogging();

// Подключение Swagger
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

var app = builder.Build();

app.UseStaticFiles();

app.UseCors("default");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapHub<NotificationHub>("/notification-hub");

app.UseSwagger();
app.UseSwaggerUI();

// var users = new List<UserData>
// {
//     new UserData { TotalOrder = 15, TotalPurchases = 30, TotalSpent = 500, AdClicks = 5, LoginFrequency = 1.2f, AverageSessionDuration = 3600, IsLikelyToChurn = false },
//     new UserData { TotalOrder = 5, TotalPurchases = 10, TotalSpent = 200, AdClicks = 2, LoginFrequency = 0.3f, AverageSessionDuration = 600, IsLikelyToChurn = true },
// };
// 
// // Обучение
// var churnModel = new ChurnPredictionModel();
// churnModel.TrainModel(users);
// 
// // Прогнозирование
// var newUser = new UserData { TotalOrder = 8, TotalPurchases = 15, TotalSpent = 350, AdClicks = 3, LoginFrequency = 0.7f, AverageSessionDuration = 1800 };
// var prediction = churnModel.Predict(newUser);
// 
// Console.WriteLine($"Вероятность оттока: {prediction.Probability:P2}, Отток: {prediction.PredictedChurn}");

app.Run();
