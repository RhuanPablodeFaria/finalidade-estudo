using FinalidadeEstudo.Infrastructure.Persistence;
using FinalidadeEstudo.Infrastructure.Persistence.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinalidadeEstudo.CrossCutting.DependencyInjection;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
                                                               IConfiguration configuration)
    {
        var connectionString = configuration
            .GetSection(DatabaseSettings.SectionName)["ConnectionString"]
            ?? throw new InvalidOperationException(
               $"Missing configuration: '{DatabaseSettings.SectionName}:ConnectionString'");

        services.AddDbContextPool<AppDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                sql => sql.MigrationsAssembly(
                    typeof(AppDbContext).Assembly.FullName)));

        return services;
    }
}
