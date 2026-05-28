using FinalidadeEstudo.Domain.Projecao;
using System.Linq.Expressions;

namespace FinalidadeEstudo.Domain.Interfaces.Queries;

public interface IQueryBase<T> where T : class
{
    Task<T?> GetAsync(Expression<Func<T, bool>> func, CancellationToken ct);
    Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>>? func = null, CancellationToken ct = default);
    Task<ProjectionResponse> PageAsync<TResult>(ProjectionRequest<TResult> projecao, CancellationToken ct = default) where TResult : class;
}
