using FinalidadeEstudo.Domain.Interfaces.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FinalidadeEstudo.Infrastructure.Persistence.Queries;

public abstract class QueryBase<T>(AppDbContext context) : IQueryBase<T> where T : class
{
    private readonly IQueryable<T> Query = context.Set<T>().AsNoTracking();
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? func = null,
                                            CancellationToken ct = default)
    {
        if (func is not null)
            return await Query.Where(func).ToListAsync();

        return await Query.ToListAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> func, CancellationToken ct = default) =>
        await Query.FirstOrDefaultAsync(func);
}
