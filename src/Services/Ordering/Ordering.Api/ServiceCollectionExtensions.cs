using BuildingBlocks.Exceptions.Handlers;
using Carter;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter();

        services.AddExceptionHandler<CustomExceptionHandler>();
        
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("Database"));
        
        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();

        app.UseExceptionHandler(_ => { });

        app.UseHealthChecks("/health");
        
        return app;
    }
}