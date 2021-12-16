using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Rajanell.MemoryCache.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Rajanell.MemoryCache.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private IMemoryCache _cache;
        private readonly AppSettings _appSettings;

        public MemoryCacheService(IMemoryCache cache, IOptions<AppSettings> options)
        {
            _cache = cache;
            _appSettings = options.Value;
        }

        public void AddRecord<T>(string recordId, T data)
        {
            int expirationMinutes = int.Parse(_appSettings.CacheTime);
            var expirationTime = (DateTime.Now.AddMinutes(int.Parse(_appSettings.CacheTime)) - DateTime.Now);
            var expirationToken = new CancellationChangeToken(
                new CancellationTokenSource(TimeSpan.FromMinutes(expirationMinutes + .01)).Token);

            var cacheEntryOptions = new MemoryCacheEntryOptions()             
                 .SetPriority(CacheItemPriority.NeverRemove)           // Pin to cache.     
                 .SetAbsoluteExpiration(expirationTime)       // Set the actual expiration time    
                 .AddExpirationToken(expirationToken)             // Force eviction to run                
                 .RegisterPostEvictionCallback(callback: CacheItemRemoved, state: this); // Add eviction callback

            var jsonData = JsonSerializer.Serialize(data);
             _cache.Set(recordId, jsonData, cacheEntryOptions);
        }

        private void CacheItemRemoved(object key, object value, EvictionReason reason, object state)
        {
            var reasonDataIsRemoved = EvictionReason.Removed;
        }

        public T GetRecord<T>(string recordId)
        {
            _cache.TryGetValue(recordId, out string jsonData);

            if (jsonData is null)
                return default;

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
