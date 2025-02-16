using CustomerChurmPrediction.RabbitMQ;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

namespace CustomerChurmPrediction.Utils
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавление политики Cors
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
        /// Добавление сервисов WebApi
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebApiServices(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }

        /// <summary>
        /// Добавить сервисы аутентификации и авторизации
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
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
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
        /// Добавить MongoDb сервис
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
        /// Добавление сервисов RabbitMQ
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRabbitMQServices(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQService, RabbitMQService>();
            return services;
        }

        /// <summary>
        /// Добавление инфраструктурных сервисов
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

            return services;
        }

        /// <summary>
        /// Добавить CRUD-сервисы
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrudServices(this IServiceCollection services)
        {
            // Базовый сервис
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }

        /// <summary>
        /// Добавить сервисы уведомлений
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddNotificationServices(this IServiceCollection services)
        {
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }

        /// <summary>
        /// Добавить сервисы для отслеживания посещённых пользователем страниц
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddPageServices(this IServiceCollection services)
        {
            services.AddScoped<IPageService, PageService>();

            return services;
        }

        /// <summary>
        /// Добавить сервисы для работы с пользователем
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
    }
}
