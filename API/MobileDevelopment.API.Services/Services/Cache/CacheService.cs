using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Services.Cache
{
    public sealed class CacheService(IDistributedCache cache, ILogger<CacheService> logger) : ICacheService
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly IDistributedCache _cache = cache;
        private readonly ILogger<CacheService> _logger = logger;

        public async Task<T> GetOrSetAsync<T>(
            string key,
            Func<CancellationToken, Task<T>> factory,
            TimeSpan ttl,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var cached = await _cache.GetStringAsync(key, cancellationToken);
                if (!string.IsNullOrWhiteSpace(cached))
                {
                    var value = JsonSerializer.Deserialize<T>(cached, JsonOptions);
                    if (value is not null)
                    {
                        return value;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache read failed for key {CacheKey}", key);
            }

            var freshValue = await factory(cancellationToken);

            try
            {
                var payload = JsonSerializer.Serialize(freshValue, JsonOptions);
                await _cache.SetStringAsync(
                    key,
                    payload,
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = ttl },
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache write failed for key {CacheKey}", key);
            }

            return freshValue;
        }

        public async Task<T> GetOrSetVersionedAsync<T>(
            string area,
            string key,
            Func<CancellationToken, Task<T>> factory,
            TimeSpan ttl,
            CancellationToken cancellationToken = default)
        {
            var areaVersion = await GetAreaVersionAsync(area, cancellationToken);
            var versionedKey = $"{area}:v:{areaVersion}:{key}";
            return await GetOrSetAsync(versionedKey, factory, ttl, cancellationToken);
        }

        public async Task InvalidateAreaAsync(string area, CancellationToken cancellationToken = default)
        {
            var versionKey = GetAreaVersionKey(area);
            var nextVersion = Guid.NewGuid().ToString("N");

            try
            {
                await _cache.SetStringAsync(
                    versionKey,
                    nextVersion,
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) },
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache area invalidation failed for area {CacheArea}", area);
            }
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                await _cache.RemoveAsync(key, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache remove failed for key {CacheKey}", key);
            }
        }

        private async Task<string> GetAreaVersionAsync(string area, CancellationToken cancellationToken)
        {
            var versionKey = GetAreaVersionKey(area);

            try
            {
                var version = await _cache.GetStringAsync(versionKey, cancellationToken);
                if (!string.IsNullOrWhiteSpace(version))
                {
                    return version;
                }

                const string initialVersion = "0";
                await _cache.SetStringAsync(
                    versionKey,
                    initialVersion,
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) },
                    cancellationToken);

                return initialVersion;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache area version read failed for area {CacheArea}", area);
                return "fallback";
            }
        }

        private static string GetAreaVersionKey(string area) => $"cache-version:{area}";
    }
}
