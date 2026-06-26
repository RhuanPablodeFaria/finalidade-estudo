using FinalidadeEstudo.Domain.Interfaces;
using FinalidadeEstudo.Infrastructure.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace FinalidadeEstudo.CrossCutting.DependencyInjection;

public static class CacheServiceExtensions
{
    public static IServiceCollection AddCacheServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration
                                   .GetSection("RedisSettings")["ConnectionString"]
                               ?? throw new InvalidOperationException("Missing configuration: 'RedisSettings:ConnectionString'");

        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(connectionString));

        services.AddSingleton<ICacheService, RedisCacheService>();

        return services;
    }
}