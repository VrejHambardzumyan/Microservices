using EnrollmentService.Infrastructure.Persistence.Repositories;
using EnrollmentService.Application.Services;
using EnrollmentService.Application.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace EnrollmentService.Infrastructure.Config;

public static class DependencyInjection
{
    public static IServiceCollection AddEnrollment(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<EnrollmentRepository>();

        // Application services
        services.AddScoped<EnrollmentService.Application.Services.EnrollmentService>();

        // Messaging
        services.AddScoped<EnrollmentPriceRequester>();
        services.AddScoped<EnrollmentPriceResponseHandler>();

        return services;
    }
}