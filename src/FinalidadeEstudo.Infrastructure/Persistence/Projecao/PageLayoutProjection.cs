using FinalidadeEstudo.Domain.Projecao;
using Microsoft.EntityFrameworkCore;

namespace FinalidadeEstudo.Infrastructure.Persistence.Projecao;

public static class PageLayoutProjection
{
    public static async Task<ProjectionResponse> PaginarAsync<T>(
        ProjectionRequest<T> request,
        CancellationToken cancellationToken) where T : class
    {
        if (request.Queryable is not null)
        {
            var total = await request.Queryable.CountAsync(cancellationToken);
            var data = await request.Queryable
                .Skip(request.Start)
                .Take(request.OffSet)
                .ToListAsync(cancellationToken);

            return ProjectionResponse.Create(data, total);
        }

        if (request.Enumerable is not null)
        {
            var list = request.Enumerable.ToList();
            var data = list.Skip(request.Start).Take(request.OffSet);

            return ProjectionResponse.Create(data, list.Count);
        }

        return ProjectionResponse.Create(Enumerable.Empty<T>(), 0);
    }
}
