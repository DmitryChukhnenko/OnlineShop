using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using OnlineShop.Application.Interfaces;

namespace OnlineShop.Infrastructure.Services;

public class CachingService : ICachingService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<CachingService> _logger;

    public CachingService(
        IDistributedCache cache, 
        ILogger<CachingService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var value = await _cache.GetStringAsync(key);
        if (value == null) return default;
        
        _logger.LogInformation("Cache hit for {Key}", key);
        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task SetAsync<T>(
        string key, 
        T value, 
        TimeSpan? expiration = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(10)
        };
        
        await _cache.SetStringAsync(
            key, 
            JsonSerializer.Serialize(value), 
            options);
        
        _logger.LogInformation("Cache set for {Key}", key);
    }
}

