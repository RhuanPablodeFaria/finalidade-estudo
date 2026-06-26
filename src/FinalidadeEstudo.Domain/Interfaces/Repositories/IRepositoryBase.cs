using System.Linq.Expressions;

namespace FinalidadeEstudo.Domain.Interfaces.Repositories;

public interface IRepositoryBase<T> where T : class
{
    Task AddAsync(T entity, CancellationToken cancellationToken);
    Task UpdateAsync(T entity);
    Task<T?> GetAsync(Expression<Func<T, bool>> func, CancellationToken cancellationToken);
}
