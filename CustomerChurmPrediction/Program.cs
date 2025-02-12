using MongoDB.Driver;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CustomerChurmPrediction;

using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
options.AddPolicy("default", policy =>
{
    // ����� React
    policy.WithOrigins("http://localhost:3000")
          .AllowAnyHeader()
          .AllowAnyMethod()
          // ��� ������ SignalR
          .AllowCredentials();
}));

var configuration = builder.Configuration;

// ����������� � ���� ������
builder.Services.AddSingleton<IMongoClient, MongoClient>(options =>
{
    return new MongoClient(configuration["DatabaseConnection:ConnectionString"]);
});

// ����������� �����������
builder.Services.AddControllers();

// ����������� ��������������
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

    // ��������� ��� SignalR: �������� ������ ����� Query String
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // ���� ������ ������������ ��� ���� SignalR, ������� �����
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// ����������� �������� ��� �������������� � ��
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

// ����������� �����������
builder.Services.AddLogging();

// ����������� Swagger
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

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

// ���������� �� ����� ��� � ����
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
