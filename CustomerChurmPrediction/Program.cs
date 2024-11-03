using MongoDB.Driver;
using CustomerChurmPrediction.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddSingleton<IMongoClient, MongoClient>(options =>
{
    return new MongoClient(configuration["DatabaseConnection:ConnectionString"]);
});

builder.Services.AddControllers();

// Сервисы для взаимодействием с бд
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.MapControllers();

app.Run();
