using System.Linq.Expressions;

namespace FinalidadeEstudo.Application.Extension;

public static class QueryableExtension
{
    public static IQueryable<T>? SortBankInquiry<T, TKey>(
        this IQueryable<T> query,
        string Orderdirection,
        Expression<Func<T, TKey>> ordenationProperty)
    {
        if (string.IsNullOrWhiteSpace(Orderdirection) || ordenationProperty is null || query is null)
            return query;

        return Orderdirection.Equals("desc", StringComparison.OrdinalIgnoreCase) ?
               query.OrderByDescending(ordenationProperty) :
               query.OrderBy(ordenationProperty);
    }
}
