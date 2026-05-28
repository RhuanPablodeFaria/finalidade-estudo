
namespace FinalidadeEstudo.Domain.Projecao;

public class ProjectionRequest<T> where T : class
{
    public IQueryable<T> Queryable { get; private set; }
    public IEnumerable<T> Enumerable { get; private set; }
    public List<T> Data { get; private set; }
    public int Start { get; private set; }
    public int OffSet { get; private set; }
    public int Total { get; private set; }

    public ProjectionRequest(IQueryable<T> query, int start, int offSet)
    {
        Queryable = query;
        Start = start;
        OffSet = offSet;
    }

    public ProjectionRequest(IEnumerable<T> enumerable, int start, int offSet)
    {
        Enumerable = enumerable;
        Start = start;
        OffSet = offSet;
    }
}
