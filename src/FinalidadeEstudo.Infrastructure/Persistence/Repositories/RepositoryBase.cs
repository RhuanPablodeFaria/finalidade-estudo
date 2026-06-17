using FinalidadeEstudo.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FinalidadeEstudo.Infrastructure.Persistence.Repositories;

internal class RepositoryBase<T>(AppDbContext context) : IRepositoryBase<T> where T : class
{
    private readonly AppDbContext Context = context;
    protected readonly DbSet<T> Entity = context.Set<T>();

    public async Task AddAsync(T entity, CancellationToken cancellationToken) =>
        await Entity.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(T entity)
    {
        Entity.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> func, CancellationToken cancellationToken) =>
        await Entity.FirstOrDefaultAsync(func, cancellationToken);
}
