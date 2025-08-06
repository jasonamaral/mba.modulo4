using BFF.Application.Interfaces.Services;
using BFF.Domain.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace BFF.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache? _distributedCache;
    private readonly IMemoryCache? _memoryCache;
    private readonly RedisSettings _redisSettings;
    private readonly ILogger<CacheService> _logger;
    private readonly bool _useDistributedCache;

    public CacheService(
        IServiceProvider serviceProvider,
        IOptions<RedisSettings> redisOptions,
        ILogger<CacheService> logger)
    {
        _redisSettings = redisOptions.Value;
        _logger = logger;

        // Tenta usar distributed cache (Redis) primeiro, senão usa memory cache
        _distributedCache = serviceProvider.GetService<IDistributedCache>();
        _memoryCache = serviceProvider.GetService<IMemoryCache>();
        
        _useDistributedCache = _distributedCache != null && 
                              !string.IsNullOrEmpty(_redisSettings.ConnectionString);

        _logger.LogInformation("Cache configurado: {CacheType}", 
            _useDistributedCache ? "Redis (Distributed)" : "Memory");
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        var fullKey = GetFullKey(key);
        
        try
        {
            if (_useDistributedCache && _distributedCache != null)
            {
                var cachedValue = await _distributedCache.GetStringAsync(fullKey);
                if (!string.IsNullOrEmpty(cachedValue))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    
                    return JsonSerializer.Deserialize<T>(cachedValue, options);
                }
            }
            else if (_memoryCache != null)
            {
                return _memoryCache.Get<T>(fullKey);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar item do cache: {Key}. Erro: {Error}", fullKey, ex.Message);
            // Remove o item corrompido do cache
            await RemoveAsync(key);
        }

        return null;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
    {
        var fullKey = GetFullKey(key);
        var exp = expiration ?? _redisSettings.DefaultExpiration;
        
        try
        {
            if (_useDistributedCache && _distributedCache != null)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };
                
                var serializedValue = JsonSerializer.Serialize(value, options);
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = exp
                };
                
                await _distributedCache.SetStringAsync(fullKey, serializedValue, cacheOptions);
            }
            else if (_memoryCache != null)
            {
                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = exp
                };
                
                _memoryCache.Set(fullKey, value, options);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar item no cache: {Key}. Erro: {Error}", fullKey, ex.Message);
        }
    }

    public async Task RemoveAsync(string key)
    {
        var fullKey = GetFullKey(key);
        
        try
        {
            if (_useDistributedCache && _distributedCache != null)
            {
                await _distributedCache.RemoveAsync(fullKey);
            }
            else if (_memoryCache != null)
            {
                _memoryCache.Remove(fullKey);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover item do cache: {Key}", fullKey);
        }
    }

    public async Task RemovePatternAsync(string pattern)
    {
        // Para memory cache, não há suporte nativo a patterns
        // Para Redis, seria necessário usar StackExchange.Redis diretamente
        _logger.LogWarning("RemovePatternAsync não implementado para o tipo de cache atual");
        await Task.CompletedTask;
    }

    private string GetFullKey(string key)
    {
        return $"{_redisSettings.KeyPrefix}{key}";
    }
} 