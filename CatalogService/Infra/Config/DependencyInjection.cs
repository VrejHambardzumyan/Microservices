using CatalogService.Infrastructure.Persistence.Repositories;
using CatalogService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Infrastructure.Config;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalog(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<ItemRepository>();
        services.AddScoped<PriceRepository>();

        // Application services
        services.AddScoped<ItemService>();
        services.AddScoped<PriceService>();

        return services;
    }
}