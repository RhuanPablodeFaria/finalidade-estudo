using FinalidadeEstudo.Domain.Interfaces.Queries;
using FinalidadeEstudo.Domain.Projecao;
using FinalidadeEstudo.Infrastructure.Persistence.Projecao;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FinalidadeEstudo.Infrastructure.Persistence.Queries;

public abstract class QueryBase<T>(AppDbContext context) : IQueryBase<T> where T : class
{
    private readonly IQueryable<T> Query = context.Set<T>().AsNoTracking();
    public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>>? func = null)
    {
        if (func is not null)
            return Query.Where(func);

        return Query;
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> func, CancellationToken cancellationToken) =>
        await Query.FirstOrDefaultAsync(func, cancellationToken);

    public async Task<ProjectionResponse> PageAsync<TResult>(ProjectionRequest<TResult> projecao, CancellationToken cancellationToken) where TResult : class
    {
        var result = await PageLayoutProjection.PaginarAsync(projecao, cancellationToken);
        return result;
    }

}
