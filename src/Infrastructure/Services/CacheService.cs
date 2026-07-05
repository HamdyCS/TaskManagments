using Application.Common.Exceptions;
using Application.Common.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class CacheService(IDistributedCache distributedCache, ILogger<CacheService> logger) : ICacheService
    {
        public async Task<T?> GetAsync<T>(string key, bool throwOnFailure = false)
        {
            try
            {
                var value = await distributedCache.GetStringAsync(key);
                return value == null ? default : JsonSerializer.Deserialize<T>(value);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, message: "Failed to get cache item for key {key}", key);

                return throwOnFailure ? throw new CacheOperationException(ex) : default;
            }
        }

        public async Task RemoveAsync(string key, bool throwOnFailure = false)
        {
            try
            {
                await distributedCache.RemoveAsync(key);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, message: "Failed to Remove cache item for key {key}", key);

                if (throwOnFailure)
                    throw new CacheOperationException(ex);

            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan AbsoluteExpiration, bool throwOnFailure = false)
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.Add(AbsoluteExpiration)
                };

                var serializedValue = JsonSerializer.Serialize(value);
                await distributedCache.SetStringAsync(key, serializedValue, options);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, message: "Failed to set cache item for key {key} and value {value}", key, value);

                if (throwOnFailure)
                    throw new CacheOperationException(ex);
            }
        }

    }
}
