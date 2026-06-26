using System.Text.Json;
using FinalidadeEstudo.Domain.Interfaces;
using StackExchange.Redis;

namespace FinalidadeEstudo.Infrastructure.Cache;

public sealed class RedisCacheService(IConnectionMultiplexer connection) : ICacheService
{
    private readonly IDatabase _database = connection.GetDatabase();

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        var value = await _database.StringGetAsync(key);

        return value.IsNullOrEmpty ?
            default :
            JsonSerializer.Deserialize<T>((string)value!, _jsonOptions);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        CancellationToken cancellationToken,
        TimeSpan? expiration = null)
    {
        var serialized = JsonSerializer.Serialize(value, _jsonOptions);

        await _database.StringSetAsync(
            key,
            serialized,
            expiration ?? TimeSpan.FromMinutes(5));
    }
}