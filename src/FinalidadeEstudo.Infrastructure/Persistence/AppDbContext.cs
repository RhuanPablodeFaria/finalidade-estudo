using Microsoft.EntityFrameworkCore;

namespace FinalidadeEstudo.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> option) : DbContext(option)
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {



        base.OnModelCreating(modelBuilder);
    }

}
