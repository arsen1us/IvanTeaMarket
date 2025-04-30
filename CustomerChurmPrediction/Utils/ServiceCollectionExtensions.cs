using CustomerChurmPrediction.Services;
using CustomerChurmPrediction.Services.BackgroundServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

namespace CustomerChurmPrediction.Utils
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Подключает и настраивает политику CORS
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:3000",
                        "http://localhost:4000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // Для SignalR
                }));

            return services;
        }

        /// <summary>
        /// Подключает сервисы WebApi
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebApiServices(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }

        /// <summary>
        /// Подключает сервисы аутентификации и авторизации
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthenticationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Добавление сервиса аутентификации
            services.AddAuthentication(options =>
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
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["TokenSettings:Key"]))
                };

                // Настройка для SignalR: передача токена через Query String
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notification-hub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // Добавление сервиса авторизации
            services.AddAuthorization();

            // Добавление сервиса для генерации и проверки jwt-токена
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }

        /// <summary>
        /// Подключает MongoDb сервис
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddMongoDbService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoClient, MongoClient>(_ =>
                new MongoClient(configuration["DatabaseConnection:ConnectionString"]));

            return services;
        }

        /// <summary>
        /// Подключает инфраструктурные сервисы
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Подключение логирования
            services.AddLogging();
            // Подключение Swagger
            services.AddSwaggerGen();
            // Подключение SignalR
            services.AddSignalR();
            // Подключение сервиса для работы с подключениями пользователей
            services.AddSingleton<IUserConnectionService, UserConnectionService>();

            return services;
        }

        /// <summary>
        /// Подключает CRUD-сервисы
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrudServices(this IServiceCollection services)
        {
            // Базовый сервис
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITeaService, TeaService>();
            services.AddScoped<IPersonalUserBidService, PersonalUserBidService>();
            services.AddTransient<IInvoiceService, InvoiceService>();

            return services;
        }

        /// <summary>
        /// Подключает сервисы уведомлений
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddNotificationServices(this IServiceCollection services)
        {
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }

        /// <summary>
        /// Подключает сервисы для отслеживания посещённых пользователем страниц
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddPageServices(this IServiceCollection services)
        {
            services.AddScoped<IPageService, PageService>();

            return services;
        }

        /// <summary>
        /// Подключает сервисы для работы с пользователем
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUserServices(this IServiceCollection services)
        {
            services.AddScoped<IUserActionService, UserActionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISessionService, SessionService>();

            return services;
        }

        /// <summary>
        /// Подключает сервисы для работы с Telegram ботом
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddTelegramBotServices(this IServiceCollection services)
        {
            services.AddSingleton<ITelegramBotService, TelegramBotService>();
            return services;
        }

        /// <summary>
        /// Подключает и настраивает HSTS
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddStrictTransportSecurity(this IServiceCollection services)
        {
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });

            return services;
        }

        /// <summary>
        /// Подключает фоновые сервисы
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<CleaningInvoicesBackgroundService>();
            return services;
        }
    }
}
