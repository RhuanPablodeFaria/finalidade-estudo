using System.Linq.Expressions;

namespace FinalidadeEstudo.Domain.Interfaces.Repositories;

public interface IRepositoryBase<T> where T : class
{
    Task AddAsync(T entity, CancellationToken ct);
    Task UpdateAsync(T entity, CancellationToken ct);
    Task<T?> GetAsync(Expression<Func<T, bool>> func, CancellationToken ct);
}
