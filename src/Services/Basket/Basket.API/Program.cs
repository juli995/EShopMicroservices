using Basket.API.Data;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
var connectionString = builder.Configuration.GetConnectionString("Database");
var redisConnectionString = builder.Configuration.GetConnectionString("Redis");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("No database connection string found");
}

if (string.IsNullOrWhiteSpace(redisConnectionString))
{
    throw new Exception("No redis connection string found");
}

builder.Services.AddCarter();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(assembly);

    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(cfg =>
{
    cfg.Connection(connectionString);
    cfg.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString)
    .AddRedis(redisConnectionString);

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    var discountUri = builder.Configuration["GrpcSettings:DiscountUrl"];

    if (string.IsNullOrWhiteSpace(discountUri))
    {
        throw new Exception("No discount uri set in the configuration");
    }
    
    options.Address = new Uri(discountUri);
});

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler(_ => { });

app.UseHealthChecks(
    "/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    }
);

app.Run();