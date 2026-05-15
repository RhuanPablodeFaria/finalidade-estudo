using System.Linq.Expressions;

namespace FinalidadeEstudo.Domain.Interfaces.Queries;

public interface IQueryBase<T> where T : class
{
    Task<T?> GetAsync(Expression<Func<T, bool>> func, CancellationToken ct);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? func = null, CancellationToken ct = default);
}
