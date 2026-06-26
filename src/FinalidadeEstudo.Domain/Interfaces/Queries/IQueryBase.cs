using FinalidadeEstudo.Domain.Projecao;
using System.Linq.Expressions;

namespace FinalidadeEstudo.Domain.Interfaces.Queries;

public interface IQueryBase<T> where T : class
{
    Task<T?> GetAsync(Expression<Func<T, bool>> func, CancellationToken cancellationToken);
    Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>>? func = null);
    Task<ProjectionResponse> PageAsync<TResult>(ProjectionRequest<TResult> projecao, CancellationToken cancellationToken) where TResult : class;
}
