using FinalidadeEstudo.Domain.Interfaces;
using FinalidadeEstudo.Domain.Interfaces.Queries;
using FinalidadeEstudo.Infrastructure.Persistence;
using FinalidadeEstudo.Infrastructure.Persistence.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace FinalidadeEstudo.CrossCutting.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection service)
    {
        service.AddScoped<IUnitOfWork, UnitOfWork>();
        service.AddQueriesDependency();

        return service;
    }

    private static IServiceCollection AddQueriesDependency(this IServiceCollection service)
    {
        service.AddTransient<IUserQuery, UserQuery>();

        return service;
    }


}
