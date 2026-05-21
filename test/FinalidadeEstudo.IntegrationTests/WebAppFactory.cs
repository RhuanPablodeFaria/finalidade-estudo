using FinalidadeEstudo.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public sealed class WebAppFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"TestDb_{Guid.NewGuid()}";  // ← fixo por instância

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            var descriptors = services
                .Where(d => d.ServiceType.FullName!.Contains("DbContext")
                         || d.ServiceType.FullName!.Contains("EntityFramework")
                         || d.ServiceType == typeof(DbContextOptions<AppDbContext>)
                         || d.ServiceType == typeof(DbContextOptions))
                .ToList();

            foreach (var descriptor in descriptors)
                services.Remove(descriptor);


            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));  // ← mesmo nome para todas as requisições
        });
    }
}