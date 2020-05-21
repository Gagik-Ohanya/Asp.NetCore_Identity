using Enyim.Caching;
using Enyim.Caching.Memcached.Results;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Caching
{
    public class CacheProvider : ICacheProvider
    {
        private readonly IMemcachedClient _memcachedClient;
        private readonly IDistributedCache _distributedCache;

        public CacheProvider(IMemcachedClient memcachedClient, IDistributedCache distributedCache)
        {
            _memcachedClient = memcachedClient;
            _distributedCache = distributedCache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            T result;
            byte[] encodedValue = await _distributedCache.GetAsync(key);
            if (encodedValue != null)
            {
                result = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(encodedValue));
            }
            else
            {
                result = JsonConvert.DeserializeObject<T>("");     //to remove
                //get value from db
                //SetAsync(key, value);
            }
            return result;
        }

        public async Task SetAsync<T>(string key, T value)
        {
            string serializedValue = JsonConvert.SerializeObject(value);
            byte[] encodedValue = Encoding.UTF8.GetBytes(serializedValue);
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)).SetAbsoluteExpiration(DateTime.Now.AddHours(5));
            await _distributedCache.SetAsync(key, encodedValue, options);
        }

        public async Task<IGetOperationResult<T>> GetCache<T>(string key)
        {
            return await _memcachedClient.GetAsync<T>(key);
        }

        public async Task SetCache<T>(string key, T value)
        {
            await _memcachedClient.SetAsync(key, value, 3600);
        }
    }
}