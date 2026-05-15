using FinalidadeEstudo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FinalidadeEstudo.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> option) : DbContext(option)
{

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

}
