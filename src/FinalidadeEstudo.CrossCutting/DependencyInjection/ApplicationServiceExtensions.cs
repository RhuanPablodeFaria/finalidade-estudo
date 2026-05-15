using FinalidadeEstudo.Application;
using Microsoft.Extensions.DependencyInjection;

namespace FinalidadeEstudo.CrossCutting.DependencyInjection;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

        return services;
    }
}
