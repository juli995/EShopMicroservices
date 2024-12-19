using Discount.Grpc.Data;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Database");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("No sqlite connection string was provided");
}

builder.Services.AddDbContext<DiscountContext>(opts =>
{
    opts.UseSqlite(connectionString);
});

builder.Services.AddGrpc();

var app = builder.Build();

app.UseMigration();

app.MapGrpcService<DiscountService>();

app.Run();