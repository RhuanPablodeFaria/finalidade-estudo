using FinalidadeEstudo.CrossCutting.Logging;
using FinalidadeEstudo.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace FinalidadeEstudo.CrossCutting.DependencyInjection;

public static class LoggingServiceExtensions
{
    public static IServiceCollection AddLoggingServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddScoped<IAppLogger, AppLogger>();
        return services;
    }
}