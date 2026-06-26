using System.Text.Json;

namespace FinalidadeEstudo.Domain.Projecao;

public class ProjectionResponse
{
    public int TotalRegistros { get; set; }
    public string Data { get; set; } = string.Empty;

    public ProjectionResponse() { }

    public static ProjectionResponse Create<T>(IEnumerable<T> data, int total)
    {
        return new ProjectionResponse
        {
            TotalRegistros = total,
            Data = JsonSerializer.Serialize(data)
        };
    }

    public T? GetData<T>()
    {
        if (string.IsNullOrWhiteSpace(Data))
            return default;

        return JsonSerializer.Deserialize<T>(Data);
    }
}
